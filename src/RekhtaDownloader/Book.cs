using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using RekhtaDownloader.Models;

namespace RekhtaDownloader
{
    internal class Book
    {
        private readonly List<Page> _pages = new List<Page>();

        private readonly BlockingCollection<Page> _jobs = new BlockingCollection<Page>();

        private readonly string _bookUrl;
        private readonly int _threadCount;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;

        private string _bookId;

        private int _pageCount;

        public string BookName { get; private set; }

        public IEnumerable<Page> Pages => _pages.OrderBy(p => p.PageIndex);

        private object _lock = new object();

        private int _completeCount = 0;

        private string _outputDirectory = String.Empty;

        public Book(string bookUrl, int threadCount, ILogger logger, CancellationToken cancellationToken)
        {
            _bookUrl = bookUrl;
            _threadCount = threadCount;
            _logger = logger;
            _cancellationToken = cancellationToken;
        }

        public async Task DownloadBook(string outputPath)
        {
            var pageContents = await HttpHelper.GetTextBody(_bookUrl);
            _bookId = FindTextBetween(pageContents, "var bookId = ", ";")?.Trim().Trim('"', '\'');
            _logger.Log($"Book Id : {_bookId}");

            var actualUrl = FindTextBetween(pageContents, "var actualUrl =", ";")?.Trim().Trim('"', '\'');
            BookName = actualUrl?.ToLower().Replace("/ebooks/", "").Trim().Trim('/', '\\');
            _logger.Log($"Book Name : {BookName}");

            _pageCount = int.Parse(FindTextBetween(pageContents, "var totalPageCount =", ";")?.Trim().Trim('"', '\'') ?? throw new InvalidOperationException("Unable to parse total page count"));
            _logger.Log($"Page Count: {_pageCount}");

            _outputDirectory = Path.Combine(outputPath, _bookId.ToSafeFilename());

            _outputDirectory.EnsureEmptyDirectory();

            var pages = StringToStringArray(FindTextBetween(pageContents, "var pages = [", "];"));
            var pageIds = StringToStringArray(FindTextBetween(pageContents, "var pageIds = [", "];"));

            var tasks = new ConsumerStarter().StartAsyncConsumers(_threadCount, _cancellationToken, DownloadPage);

            for (var i = 0; i < _pageCount; i++)
            {
                _jobs.Add(new Page { PageId = pageIds[i], PageNumber = Path.GetFileNameWithoutExtension(pages[i]), FileName = pages[i] });
            }

            _jobs.CompleteAdding();
            Task.WaitAll(tasks.ToArray(), _cancellationToken);
        }

        private void DownloadPage()
        {
            foreach (var page in _jobs.GetConsumingEnumerable(_cancellationToken))
            {
                new RetryPolicyProvider(_logger).PageRetryPolicy.ExecuteAsync(async () =>
                {
                    var data = await HttpHelper.GetTextBody($"https://ebooksapi.rekhta.org/api_getebookpagebyid/?pageid={page.PageId}");
                    page.PageData = JsonConvert.DeserializeObject<PageData>(data);

                    var pageImage = await HttpHelper.GetImage($"https://ebooksapi.rekhta.org/images/{_bookId}/{page.FileName}");
                    pageImage = ImageHelper.RearrangeImage(pageImage, page.PageData);

                    var filePath = Path.Combine(_outputDirectory, page.FileName);
                    _outputDirectory.CreateIfDirectoryDoesNotExists();
                    filePath.MakeSureFileDoesNotExist();

                    pageImage.Save(filePath, ImageFormat.Jpeg);

                    page.PageImagePath = filePath;

                    lock (_lock)
                    {
                        _pages.Add(page);
                        _completeCount++;
                        _logger.Log($"Downloaded page {_completeCount} of {_pageCount}");
                    }
                }).Wait(_cancellationToken);
            }
        }

        private string FindTextBetween(string source, string start, string end)
        {
            var startIndex = source.IndexOf(start);
            var endIndex = source.IndexOf(end, startIndex);
            return source.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
        }

        private string[] StringToStringArray(string input)
        {
            var result = new List<string>();
            var items = input.Split(',');
            foreach (var item in items)
            {
                result.Add(item.Trim().Trim('"'));
            }

            return result.ToArray();
        }
    }
}