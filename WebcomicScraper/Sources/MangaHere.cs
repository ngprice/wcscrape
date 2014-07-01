﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WebcomicScraper.Comic;
using HtmlAgilityPack;

namespace WebcomicScraper.Sources
{
    public sealed class MangaHere : ISource
    {
        public string FindTitle(HtmlDocument doc)
        {
            var result = String.Empty;

            var titleElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div/h1[@class='title']");
            if (titleElement != null)
                result = titleElement.InnerText.Trim();

            return result;
        }

        public string FindDescription(HtmlDocument doc)
        {
            var result = String.Empty;

            var descriptionElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//p[@id='show']");
            if (descriptionElement != null)
            {
                descriptionElement.RemoveChild(descriptionElement.LastChild);
                result = System.Net.WebUtility.HtmlDecode(descriptionElement.InnerText.Trim());
            }
            return result;
        }

        public string FindAuthor(HtmlDocument doc)
        {
            var result = String.Empty;

            var authorElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Author(s):']/../a[1]");
            if (authorElement != null)
                result = authorElement.InnerText.Trim();

            return result;
        }

        public string FindArtist(HtmlDocument doc)
        {
            var result = String.Empty;

            var artistElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[1]/ul//label[text()='Artist(s):']/../a[1]");
            if (artistElement != null)
                result = artistElement.InnerText.Trim();

            return result;
        }

        public string FindCover(HtmlDocument doc)
        {
            var result = String.Empty;

            var coverElement = doc.DocumentNode.SelectSingleNode("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='manga_detail_top clearfix']/img[@class='img' and contains(@src,'cover')]");
            if (coverElement != null)
                result = coverElement.GetAttributeValue("src", "");

            return result;
        }

        public List<Chapter> FindChapters(HtmlDocument doc)
        {
            var result = new List<Chapter>();

            foreach (var node in doc.DocumentNode.SelectNodes("/section[@id='main']/article[1]/div[1]/div[@class='manga_detail']/div[@class='detail_list']/ul[1]/li"))
            {
                var chapter = new Chapter();
                var date = new DateTime();

                var anchorNode = node.SelectSingleNode("span/a[@class][@href]");
                if (anchorNode != null)
                {
                    chapter.Title = anchorNode.InnerText;
                    chapter.SourceURL = anchorNode.GetAttributeValue("href", "");
                }

                var newNode = node.SelectSingleNode("i[@class='new']");
                var titleNode = node.SelectSingleNode("span[@class='left']");
                if (newNode != null)
                    chapter.Description = newNode.InnerText;
                else if (titleNode != null)
                    chapter.Description = titleNode.LastChild.InnerText;

                var dateNode = node.SelectSingleNode("span[@class='right']");
                if (dateNode != null)
                {
                    DateTime.TryParse(dateNode.InnerText, out date);
                    chapter.DatePublished = date;
                }

                result.Add(chapter);

                //table of contents
                var contentsNodes = doc.DocumentNode.SelectNodes("html/body/section[1]/div[@class='go_page clearfix']/span[@class='right']/select[1]/option");
                chapter.Pages = GetPages(contentsNodes);
            }

            return result;
        }

        public List<Page> GetPages(HtmlNodeCollection nodes)
        {
            var result = new List<Page>();

            nodes.AsParallel().AsOrdered().ForAll(node =>
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string pageHtml = webClient.DownloadString(node.GetAttributeValue("value", ""));

                        var doc = new HtmlDocument();
                        doc.LoadHtml(pageHtml);

                        var imgElement = doc.DocumentNode.SelectSingleNode("html/body/section[@class='read_img'][@id='viewer']/a[1]/img");
                        if (imgElement != null)
                        {
                            var page = new Page();
                            page.PageNum = int.Parse(node.NextSibling.InnerText);
                            page.ImageURL = imgElement.GetAttributeValue("src", "");

                            result.Add(page);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error getting pages: " + ex.Message);
                    }
                };
            });

            return result.ToList();
        }
    }
}