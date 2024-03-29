﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using RekhtaDownloader.Models;
using Image = iText.Layout.Element.Image;
using Path = System.IO.Path;

namespace RekhtaDownloader
{
    public class BookExporter
    {
        private readonly ILogger _logger;

        public BookExporter(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<BookInfo> GetBookInformation(string bookUrl, CancellationToken token = default(CancellationToken))
        {
            var book = new Book(bookUrl, 1, _logger, token);
            return await book.GetBookInformation();
        }

        public async Task<string> DownloadBook(string bookUrl, int taskCount = 10, OutputType output = OutputType.Pdf, string outputPath = null, CancellationToken token = default(CancellationToken))
        {
            var book = new Book(bookUrl, taskCount, _logger, token);
            var workingFolder = outputPath ?? Environment.CurrentDirectory;
            await book.DownloadBook(workingFolder);

            if (!book.Pages.Any())
            {
                _logger.Log("No pages were downloaded for book");
                return null;
            }

            if (output == OutputType.Pdf)
            {
                return ExportPdf(book, workingFolder);
            }

            if (output == OutputType.Images)
            {
                return ExportImages(book, workingFolder);
            }

            return null;
        }

        private string ExportPdf(Book book, string outputPath)
        {
            var firstPagePath = book.Pages.FirstOrDefault()?.PageImagePath;

            if (string.IsNullOrWhiteSpace(firstPagePath))
            {
                _logger.Log("No pages downloaded to create Pdf.");
                return null;
            }

            _logger.Log("Pages Downloaded. Creating Pdf...");

            var pdfPath = Path.Combine(outputPath, book.BookName.ToSafeFilename() + ".pdf");

            pdfPath.MakeSureFileDoesNotExist();

            PageSize documentSize;
            using (var firstImage = new Bitmap(firstPagePath))
            {
                documentSize = new PageSize(firstImage.Width, firstImage.Height);
            }

            using (var pdf = new PdfDocument(new PdfWriter(pdfPath)))
            {
                using (var writer = pdf.GetWriter())
                {
                    writer.SetCompressionLevel(9);

                    using (var document = new Document(pdf, documentSize))
                    {
                        document.SetMargins(0, 0, 0, 0);

                        foreach (var page in book.Pages)
                        {
                            var imageData = ImageDataFactory.Create(page.PageImagePath);
                            var image = new Image(imageData).SetHeight(imageData.GetHeight()).SetWidth(imageData.GetWidth());
                            document.Add(image);
                        }
                    }
                }
            }

            Path.GetDirectoryName(firstPagePath)?.TryDeleteDirectory();

            _logger.Log($"Book saved as {pdfPath}");

            return pdfPath;
        }

        private string ExportImages(Book book, string outputPath)
        {
            var bookDir = Path.Combine(outputPath, book.BookName.ToSafeFilename());

            try
            {
                bookDir.EnsureEmptyDirectory();

                _logger.Log($"Saving book pages in folder {bookDir}");

                foreach (var page in book.Pages.OrderBy(p => p.PageIndex))
                {
                    File.Move(page.PageImagePath, Path.Combine(bookDir, page.FileName));
                }

                Path.GetPathRoot(book.Pages.First().PageImagePath)?.TryDeleteDirectory();

                Path.GetDirectoryName(book.Pages.First().PageImagePath).TryDeleteDirectory();

                _logger.Log($"Book pages saved in folder {bookDir}");

                return bookDir;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                _logger.Log($"Error: Unable to copy pages to book folder. Pages were downloaded in {Path.GetDirectoryName(book.Pages.First().PageImagePath)}");

                return null;
            }
        }
    }
}