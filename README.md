# Rekhta Downloader

A library to download books from rekhta website.

## Build Status

[![Build status](https://ci.appveyor.com/api/projects/status/j6psbb2mlycchsl7/branch/master?svg=true)](https://ci.appveyor.com/project/umerfaruk/rekhtadownloader)

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

3. authKeyName

This is the name of the key used as part of request sent to rekhta to call the page api. See details on how to get these details in section below.

4. authKey

This is the value of auth key send to rekhta to call for page api. See details on how to get these details in section below.

3. OutputType

Specifies the type of output wanted. Possible values are `OutputType.Pdf` and `OutputType.Images`. In case of Pdf the output will be created in the root folder of application with name same as name of book. Images will be stored in a folder with name matching book.


### Logging

Given example logs all output to the console. If you want to log output to some other destination, extend the [ILogger](src/RekhtaDownloader/ILogger.cs) interface and pass it in the constructor. 

### Getting Rekhta Key name and value

1. Open the url specified in the URL parameter in browser of your choice.
2. Open the browser dev tools and reload page
3. In the network tab search for `getebookpagebyid`
4. Select any of requests get the querystring parameters. These are part of url after `?` or you can click on request to see details, scroll down to rquest and you should see key value pairs
5. One argument in query string parameters would be `pgid`, ignore it. The other argument is the one you are looking for. Use the key and value for the `authKeyName` and `authKey` parametrs respectively.

