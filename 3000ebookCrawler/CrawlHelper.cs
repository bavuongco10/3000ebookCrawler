using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace _3000ebookCrawler
{
    internal class CrawlHelper
    {
        private string _baseUrl;

        public CrawlHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public static HtmlDocument ParseHtmlFromUrl(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }

        public static Uri JoinUrl(string baseUrl, string path)
        {
            Uri baseUri = new Uri(baseUrl);
            return new Uri(baseUri, path);
        }
        public List<string> GetBoxUrListFromPage(string pageUrl)
        {
            var doc = ParseHtmlFromUrl(pageUrl);
            return
                doc.DocumentNode.SelectNodes("//*[@class='list_box_title list_title']/a")
                    .Select(element => JoinUrl(_baseUrl, element.Attributes["href"].Value).ToString())
                    .ToList();
        }

        public List<string> GetCategoriesUrl()
        {
            HtmlDocument doc = ParseHtmlFromUrl(_baseUrl);
            HtmlNode categoryPanel = doc.DocumentNode.SelectSingleNode("html/body/div[1]/div[2]/div[2]/div[3]");
            HtmlNode catrgoryTable = categoryPanel.ChildNodes["table"];
            return catrgoryTable.SelectNodes("..//a[@href]").Select(
                element => JoinUrl(_baseUrl, element.Attributes["href"].Value).ToString())
                .ToList();
        }
        
        public List<string> GetPagitationPages(string url)
        {
            var doc = ParseHtmlFromUrl(url);
            var changePageNode = doc.DocumentNode.SelectNodes("//*[@class='change_page']/a");
            HtmlNode lastOrDefault = changePageNode.LastOrDefault();
            if (lastOrDefault == null) return null;
            var lastPageUrl = lastOrDefault.Attributes["href"].Value;
            MatchCollection matchList = Regex.Matches(lastPageUrl, @"(?:\d*\.)?\d+");
            var paginationInfo = matchList.Cast<Match>().Select(match => int.Parse(match.Value)).ToList();
            var pages = new List<string>();
            for (int i = 1; i <= paginationInfo[1]; i++)
            {
                var path = $"list_{paginationInfo[0]}_{i}.html";
                pages.Add(JoinUrl(url, path).ToString());
            }
            return pages;
        }


        public string GetDownloadUrl(string contentUrl)
        {
            var doc = ParseHtmlFromUrl(contentUrl);
            var node = doc.DocumentNode.SelectSingleNode("html/body/div[1]/div[2]/div[1]/div[4]/p[1]/a");
            return node.Attributes["href"].Value;
        }
    }
}