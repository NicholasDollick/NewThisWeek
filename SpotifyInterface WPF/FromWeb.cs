using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpotifyInterface_WPF
{
    class FromWeb
    {
        private static List<string> tracks;

        public static List<string> GetData(String url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                var test = doc.DocumentNode.SelectNodes("//div[@class='s1knm1ot-5 gGDEPn s1hmcfrd-0 ckueCN']").Elements().ToList();
                foreach (HtmlNode node in test)
                {
                    tracks.Add(Regex.Replace(node.InnerText, @"\&#x27;", "'")); //remove artifacts and replaces with proper char
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return tracks;
        }

    }
}
