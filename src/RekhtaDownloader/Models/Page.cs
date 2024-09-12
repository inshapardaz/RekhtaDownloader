using System;
using System.IO;

namespace RekhtaDownloader.Models
{
    public class Page
    {
        public string PageNumber { get; set; }

        public string FileName { get; set; }

        public string PageId { get; set; }

        public PageData PageData { get; set; }

        //public Bitmap PageImage { get; set; }

        public  string PageImagePath { get; set; }

        public int PageIndex => int.Parse(Path.GetFileNameWithoutExtension(this.PageNumber) ?? throw new InvalidOperationException($"{PageNumber} is not a number"));

        public string FolderName { get; internal set; }
        public int Index { get; internal set; }
    }
}