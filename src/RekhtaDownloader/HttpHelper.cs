﻿using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace RekhtaDownloader
{
    internal static class HttpHelper
    {
        public static async Task<string> GetTextBody(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            myRequest.Accept = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            myRequest.Referer = "https://www.rekhta.org/";

            WebResponse myResponse = await myRequest.GetResponseAsync();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string result = await sr.ReadToEndAsync();
            sr.Close();
            myResponse.Close();

            return result;
        }

        public static async Task<SKImage> GetImage(string imageUrl)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;

            using (var webResponse = await webRequest.GetResponseAsync())
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    return SKImage.FromEncodedData(stream);
                }
            }
        }
    }
}