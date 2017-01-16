using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace _3000ebookCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            string ebook3000url = "http://www.ebook3000.com";
            CrawlHelper crawlHelper = new CrawlHelper(ebook3000url);
            var categories = crawlHelper.GetCategoriesUrl();
            foreach (var category in categories)
            {
                var pages = crawlHelper.GetPagitationPages(category);
                foreach (var page in pages)
                {
                    var boxurls = crawlHelper.GetBoxUrListFromPage(page);
                    foreach (var boxurl in boxurls)
                    {
                        var downloadUrl = crawlHelper.GetDownloadUrl(boxurl);
                        Console.WriteLine(downloadUrl);
                    }
                }
            }
        }
    }
}
