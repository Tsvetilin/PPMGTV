using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Common.Constants;

namespace Common.Helpers
{
    public static class SitemapFactory
    {
        public static List<SitemapNode> SitemapNodes { get; set; } = new List<SitemapNode>();
        public static IWebHostEnvironment Environment { get; set; }

        public static void UpdateSitemap(IWebHostEnvironment env = null)
        {
            if (env != null)
            {
                Environment = env;
            }
            new SitemapDocument().CreateSitemapXML(SitemapNodes, Environment.ContentRootPath);
        }

        public static void Create(IWebHostEnvironment env)
        {
            Environment = env;

            var nodes = new List<SitemapNode>()
            {
                new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.9,
                    Url = $"{SystemNames.BaseUrl}",
                    Frequency = SitemapFrequency.Monthly
                },
                new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.8,
                    Url = $"{SystemNames.BaseUrl}/videos",
                    Frequency = SitemapFrequency.Monthly
                },
                new SitemapNode
                {
                    LastModified=DateTime.UtcNow,
                    Priority=0.7,
                    Url=$"{SystemNames.BaseUrl}/gallery",
                    Frequency=SitemapFrequency.Monthly
                },
                new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.7,
                    Url = $"{SystemNames.BaseUrl}/team",
                    Frequency = SitemapFrequency.Yearly
                },
                new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.6,
                    Url = $"{SystemNames.BaseUrl}/contact",
                    Frequency = SitemapFrequency.Never
                },
                new SitemapNode
                {
                    LastModified = DateTime.UtcNow,
                    Priority = 0.3,
                    Url = $"{SystemNames.BaseUrl}/identity/account/login",
                    Frequency = SitemapFrequency.Never
                },
            };

            SitemapNodes.AddRange(nodes);
            UpdateSitemap();
        }

        public static void AppendSitemapNode(string url, DateTime? lastModified, double? priority = 0.5, SitemapFrequency frequency = SitemapFrequency.Never, bool updateSitemap = false, IWebHostEnvironment env=null)
        {
            if (env != null)
            {
                Environment = env;
            }

            SitemapNodes.Add
            (
                new SitemapNode
                {
                    LastModified = lastModified,
                    Priority = priority,
                    Url = url,
                    Frequency = frequency
                }
            );

            if(updateSitemap)
            {
                UpdateSitemap();
            }
        }
    }
}
