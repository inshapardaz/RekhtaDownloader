using System.Threading;
using System.Threading.Tasks;
using RekhtaDownloader;
using Microsoft.Extensions.CommandLineUtils;
using Common;

namespace Downloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);
            commandLineApplication.Name = "Rekhta download tool";
            commandLineApplication.Description = "To download the rekhta books";
            commandLineApplication.HelpOption("-? | -h | --help");

            CommandOption bookUrl = commandLineApplication.Option("-u |--url",
                                                                  "Book url to download. Click on book on site and copy the url from top.",
                                                                  CommandOptionType.SingleValue);

            CommandOption taskCount = commandLineApplication.Option("-t |--tasks",
                                                                    "Number of parallel pages to get. Default will be 10 pages",
                                                                    CommandOptionType.SingleValue);

            CommandOption outputType = commandLineApplication.Option("-o |--output",
                                                                  "Type of output. Possible values are 'pdf' and 'image'. Default is pdf",
                                                                  CommandOptionType.SingleValue);

            commandLineApplication.OnExecute(async () =>
            {
                using var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                if (bookUrl.HasValue())
                {
                    if (!int.TryParse(taskCount.Value(), out var tasks))
                    {
                        tasks = 10;
                    }

                    var output = OutputType.Pdf;
                    if (outputType.HasValue())
                    {
                        switch (outputType.Value().ToLower())
                        {
                            case "pdf":
                                output = OutputType.Pdf;
                                break;

                            case "image":
                                output = OutputType.Images;
                                break;

                            default:
                                commandLineApplication.ShowHelp();
                                return 0;
                        }
                    }

                    await DownloadBook(bookUrl.Value(), tasks, output, token);
                    return 0;
                }

                commandLineApplication.ShowHelp();
                return 0;
            });

            commandLineApplication.Execute(args);
        }

        private static async Task DownloadBook(string bookUrl, int taskCount, OutputType outputType, CancellationToken token)
        {
            //var bookUrl = "https://rekhta.org/ebooks/alfaz-shumara-number-000-jameel-akhtar-magazines-7/";
            await new BookExporter(new ConsoleLogger()).DownloadBook(bookUrl, taskCount, outputType);
        }
    }
}