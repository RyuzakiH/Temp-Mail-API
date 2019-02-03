using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempMail.API
{
    public static class HtmlAgilityPackExtensions
    {

        public static HtmlNode GetElementById(this HtmlNode node, string tagName, string id)
        {
            return node.Descendants(tagName).FirstOrDefault(n => n.GetAttributeValue("id", "") == id);
        }

        public static HtmlNode GetElementById(this HtmlNode node, string id)
        {
            return node.Descendants().FirstOrDefault(n => n.GetAttributeValue("id", "") == id);
        }

        public static IEnumerable<HtmlNode> GetElementsByClassName(this HtmlNode node, string tagName, string className)
        {
            return node.Descendants(tagName).Where(n => n.GetAttributeValue("class", "").Split(' ').Contains(className));
        }

        public static IEnumerable<HtmlNode> GetElementsByClassName(this HtmlNode node, string className)
        {
            return node.Descendants().Where(n => n.GetAttributeValue("class", "").Split(' ').Contains(className));
        }

    }
}
