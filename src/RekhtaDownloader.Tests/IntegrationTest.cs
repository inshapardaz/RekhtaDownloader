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
            var outputPath = await downloader.DownloadBook("https://www.rekhta.org/ebooks/rafeeq-e-manzil-shumara-number-10-magazines", 10, "authtknkey", "eyJpdiI6IktVUGJ2b2RPWG1YMStZRnRhMVhIN1E9PSIsInZhbHVlIjoiSm5ZT3BRd0J3ckxTR0Zqc3VZa3RLUT09IiwibWFjIjoiYjkyYWMxZGNlZmFiMzZjNjY5MmI3YTI4Y2UwZWU2MjQ5NzMyMDNiMjlmMTRjM2ViZjk4YTk3Y2QyZDAxMTczZiJ9", OutputType.Pdf);
        }
    }
}