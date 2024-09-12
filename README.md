# Rekhta Downloader

A library to download books from rekhta website.

## Build Status

[![.NET](https://github.com/inshapardaz/RekhtaDownloader/actions/workflows/dotnet.yml/badge.svg)](https://github.com/inshapardaz/RekhtaDownloader/actions/workflows/dotnet.yml)

[![NuGet version](https://img.shields.io/nuget/v/RekhtaDownloader.svg)](https://www.nuget.org/packages/RekhtaDownloader/)


## Usage

Add reference to your c# application. You can access the functionaltiy using `BookExporter` class. Create an instance and call download function using the following code snippet:

``` c#
var downloader = new BookExporter(new ConsoleLogger());
await downloader.DownloadBook(url, taskCount, authtkeyName, authKey, OutputType.Pdf, CancellationToken.None);

```

### Parameters

1. url

This is the url of the rekhta book. This is not the rekhta books main page but for the page where you can see the actual pages from book.

2. taskCount

Integer value telling how many parallal threads to use for downloading book.

3. OutputType

Specifies the type of output wanted. Possible values are `OutputType.Pdf` and `OutputType.Images`. In case of Pdf the output will be created in the root folder of application with name same as name of book. Images will be stored in a folder with name matching book.
