namespace ConsoleApplication1
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System;

    class Program
    {


        public class Cookie
        {
            [Newtonsoft.Json.JsonProperty(PropertyName = "CN")]
            public string CN { get; set; }

            [Newtonsoft.Json.JsonProperty(PropertyName = "PN")]
            public string PN { get; set; }

            [Newtonsoft.Json.JsonProperty(PropertyName = "VL")]
            public string VL { get; set; }
        }

        public class CustomCookieList
        {
            public string MCH { get; set; }
            public string MC1 { get; set; }
            public string ANON { get; set; }
            public string MUID { get; set; }
        }
        private static CustomCookieList ExtractCookie(string rawJSON)
        {
            CustomCookieList cookieList = new CustomCookieList();
            if (!string.IsNullOrEmpty(rawJSON))
            {
                //string modifiedCustomTag = Regex.Replace(rawJSON, @"([^: \\])'([^,} ])", "$1$2", RegexOptions.IgnoreCase);
                //modifiedCustomTag = Regex.Replace(rawJSON, @"(\\{1,})[v|u|a|b|t|r|f|n|e|x|c|\s]", "", RegexOptions.IgnoreCase);
                var modifiedCustomTag = Regex.Replace(rawJSON, @"#TAB#", ",");

                if (!string.IsNullOrEmpty(modifiedCustomTag))
                {
                    List<Cookie> cookies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cookie>>(modifiedCustomTag);
                    if (cookies == null)
                    {
                        return cookieList;
                    }
                    Cookie mc1 = cookies.Find(x => x.CN.ToLower() == "mc1");
                    Cookie anon = cookies.Find(x => x.CN.ToLower() == "anon");
                    Cookie muid = cookies.Find(x => x.CN.ToLower() == "muid");
                    Cookie token = cookies.Find(x => x.CN.ToLower() == "_mkto_trk");
                    cookieList.MC1 = mc1 == null ? string.Empty : mc1.VL;
                    cookieList.ANON = anon == null ? string.Empty : anon.VL;
                    cookieList.MUID = muid == null ? string.Empty : muid.VL;
                    cookieList.MCH = token == null ? string.Empty : token.VL.Substring(token.VL.LastIndexOf(":") + 1);
                }
            }
            return cookieList;
        }

        public struct DomainData
        {
            public string PageURIStem;
            public string Website;
            public int Priority;

            public DomainData(string pageURIStem, string website, int priority)
            {
                PageURIStem = pageURIStem;
                Website = website;
                Priority = priority;
            }
        }

        public struct Website
        {
            public string PageWebsite;
            public string ReferrerWebsite;

            public Website(string pageWebsite, string referrerWebsite)
            {
                PageWebsite = pageWebsite;
                ReferrerWebsite = referrerWebsite;
            }
        }

        public static Website GetWebsiteName()
        {
            int? allocadiaid = 54;
            bool f = allocadiaid == null;
            string PageDomain = "templafy.visualstudio.com",
                   PageURIStem = "/DefaultCollection/Templafy/_dashboards",
                   ReferrerDomain = "templafy.visualstudio.com",
                   ReferrerURIStem = "/DefaultCollection/Templafy/_dashboards",
                   pageWebsite = string.Empty,
                   referrerWebsite = string.Empty;
            Dictionary<string, List<DomainData>> DomainDictionary = new Dictionary<string, List<DomainData>>();
            List<DomainData> l = new List<DomainData>();
            l.Add(new DomainData(PageURIStem, "Visual Studio", 2));
            DomainDictionary.Add(PageDomain, l);

            List<DomainData> domainDataList = DomainDictionary.Where(pair => PageDomain.Contains(pair.Key)).Select(pair => pair.Value).FirstOrDefault();
            if (domainDataList != null)
            {
                pageWebsite = domainDataList.Where(tuple => tuple.Priority == 1 &&
                                   (string.IsNullOrEmpty(tuple.PageURIStem) ||
                                        (!string.IsNullOrEmpty(PageURIStem) && PageURIStem.Contains(tuple.PageURIStem))
                                   )).Select(tuple => tuple.Website).FirstOrDefault();
                if (string.IsNullOrEmpty(pageWebsite))
                {
                    pageWebsite = domainDataList.Where(tuple => tuple.Priority == 2).Select(tuple => tuple.Website).FirstOrDefault();
                }
            }
            domainDataList = DomainDictionary.Where(pair => ReferrerDomain.Contains(pair.Key)).Select(pair => pair.Value).FirstOrDefault();
            if (domainDataList != null)
            {
                referrerWebsite = domainDataList.Where(tuple => tuple.Priority == 1 &&
                                   (string.IsNullOrEmpty(tuple.PageURIStem) ||
                                        (!string.IsNullOrEmpty(ReferrerURIStem) && ReferrerURIStem.Contains(tuple.PageURIStem))
                                   )).Select(tuple => tuple.Website).FirstOrDefault();
                if (string.IsNullOrEmpty(referrerWebsite))
                {
                    referrerWebsite = domainDataList.Where(tuple => tuple.Priority == 2).Select(tuple => tuple.Website).FirstOrDefault();
                }
            }
            pageWebsite = string.IsNullOrEmpty(pageWebsite) ? string.Empty : pageWebsite;
            referrerWebsite = string.IsNullOrEmpty(referrerWebsite) ? string.Empty : referrerWebsite;

            foreach (KeyValuePair<string, List<DomainData>> pair in DomainDictionary)
            {

                List<DomainData> sortedList = pair.Value.OrderBy(o => o.Priority).ToList();
                if (string.IsNullOrEmpty(pageWebsite) && PageDomain.Contains(pair.Key))
                {
                    foreach (DomainData DomainDataValue in sortedList)
                    {
                        if (string.IsNullOrEmpty(DomainDataValue.PageURIStem) || ((PageURIStem != null) && PageURIStem.Contains(DomainDataValue.PageURIStem)))
                        {
                            pageWebsite = DomainDataValue.Website;
                            break;
                        }
                    }
                }
                //if (string.IsNullOrEmpty(referrerWebsite) && ReferrerDomain.Contains(pair.Key))
                //{
                //    foreach (DomainData DomainDataValue in sortedList)
                //    {
                //        if (string.IsNullOrEmpty(DomainDataValue.PageURIStem) || ((ReferrerURIStem != null) && ReferrerURIStem.Contains(DomainDataValue.PageURIStem)))
                //        {
                //            referrerWebsite = DomainDataValue.Website;
                //            break;
                //        }
                //    }
                //}
                if (string.IsNullOrEmpty(pageWebsite) && string.IsNullOrEmpty(referrerWebsite))
                {
                    break;
                }
            }
            return new Website(pageWebsite, referrerWebsite);
        }

        static void Main(string[] args)
        {


            Console.ReadKey();
        }
    }
}
