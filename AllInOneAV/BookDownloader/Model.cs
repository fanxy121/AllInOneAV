﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookDownloader
{
    public class Model
    {
    }

    public class BookInfo
    {
        public string BookName { get; set; }
        public List<Chapter> Chapters { get; set; }
        public CookieContainer Cookie { get; set; }
    }

    public class Chapter
    {
        public string ChapterName { get; set; }
        public string ChapterUrl { get; set; }
    }
}
