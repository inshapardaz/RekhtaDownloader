using System.Threading;
using System.Threading.Tasks;
using RekhtaDownloader;
using System.CommandLine;
using Common;

namespace Downloader
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var urlOption = new Option<string>(
                name: "--url",
                description: "Url the book page where you can read the book contents.")
                { IsRequired = true };
            urlOption.AddAlias("-u");

            var tasksOption = new Option<int>(
                name: "--tasks",
                description: "Number of parallel pages to get. Default will be 10 pages.",
                getDefaultValue: () => 10);
            tasksOption.AddAlias("-t");

            var outputOption = new Option<OutputType>(
                name: "--output",
                description: "Type of output to be generated. Options are pdf and ",
                getDefaultValue: () => OutputType.Pdf);
            outputOption.AddAlias("-o");


            var rootCommand = new RootCommand("Rekhta download tool to download the rekhta books.");
            rootCommand.AddOption(urlOption);
            rootCommand.AddOption(tasksOption);
            rootCommand.AddOption(outputOption);

            rootCommand.SetHandler(async (context) =>
            {
                string url = context.ParseResult.GetValueForOption(urlOption);
                int tasks= context.ParseResult.GetValueForOption(tasksOption);
                OutputType output = context.ParseResult.GetValueForOption(outputOption);
                var token = context.GetCancellationToken();

                await DownloadBook(url, tasks, output, token);
            });

            await rootCommand.InvokeAsync(args);

            return 0;
        }

        private static async Task DownloadBook(string bookUrl, int taskCount, OutputType outputType, CancellationToken token)
        {
            //var bookUrl = "https://rekhta.org/ebooks/alfaz-shumara-number-000-jameel-akhtar-magazines-7/";

            await new BookExporter(new ConsoleLogger()).DownloadBook(bookUrl, taskCount, outputType);
        }
    }
}