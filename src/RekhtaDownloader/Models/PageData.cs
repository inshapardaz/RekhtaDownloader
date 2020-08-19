using System;

namespace RekhtaDownloader.Models
{
    public class PageData
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int PageHeight { get; set; }

        public int PageWidth { get; set; }

        public Guid PageId { get; set; }

        public Sub[] Sub { get; set; }
    }

    public class Sub
    {
        public int X1 { get; set; }

        public int X2 { get; set; }

        public int Y1 { get; set; }

        public int Y2 { get; set; }
    }
}