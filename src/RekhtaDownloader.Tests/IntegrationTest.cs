using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace RekhtaDownloader.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public async Task TestDownload()
        {
            var logger = new ConsoleLogger();
            var downloader = new BookExporter(logger);
            var outputPath = await downloader.DownloadBook("https://www.rekhta.org/ebooks/rafeeq-e-manzil-shumara-number-10-magazines", 10, OutputType.Pdf);
        }
    }
}