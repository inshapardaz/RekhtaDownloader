using System;
using System.Drawing;
using System.IO;

namespace RekhtaDownloader.Models
{
    public class BookInfo
    {
        public string Title { get; set; }

        public string[] Authors { get; set; }

        public string Publisher { get; set; }

        public byte[] Image { get; set; }
        public int Year { get; set; }
    }
}