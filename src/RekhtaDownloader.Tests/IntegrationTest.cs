using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RekhtaDownloader.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        static readonly ILoggerFactory LoggingFactory = LoggerFactory.Create(builder => builder.AddConsole());

        [TestMethod]
        public async Task TestDownload()
        {
            var logger = LoggingFactory.CreateLogger<BookExporter>();
            var downloader = new BookExporter(logger);
            var outputPath = await downloader.DownloadBook("https://www.rekhta.org/ebooks/rafeeq-e-manzil-shumara-number-10-magazines", 10, OutputType.Pdf);
        }

        [TestMethod]
        public async Task TestGetInformation()
        {
            var logger = LoggingFactory.CreateLogger<BookExporter>();
            var downloader = new BookExporter(logger);
            var bookInfo= await downloader.GetBookInformation("https://www.rekhta.org/ebooks/sarguzisht-abdul-majeed-salik-ebooks-1");
            Assert.IsNotNull(bookInfo);
            Assert.AreEqual("Sarguzisht", bookInfo.Title);
            Assert.AreEqual("Abdul Majeed Salik", bookInfo.Authors.First());
            Assert.AreEqual("Qaumi Kutub Khana, Lahore", bookInfo.Publisher);
            Assert.AreEqual(1955, bookInfo.Year);
            Assert.IsNotNull(bookInfo.Image);
        }
    }
}