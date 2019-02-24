using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace TempMail.API.Extensions
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

        public static IEnumerable<HtmlNode> GetElementsByName(this HtmlNode node, string tagName, string name)
        {
            return node.Descendants(tagName).Where(n => n.GetAttributeValue("name", "") == name);
        }

        public static IEnumerable<HtmlNode> GetElementsByName(this HtmlDocument document, string tagName, string name)
        {
            return document.DocumentNode.Descendants(tagName).Where(n => n.GetAttributeValue("name", "") == name);
        }

        public static IEnumerable<HtmlNode> GetElementsByName(this HtmlNode node, string name)
        {
            return node.Descendants().Where(n => n.GetAttributeValue("name", "") == name);
        }

        public static IEnumerable<HtmlNode> GetElementsByName(this HtmlDocument document, string name)
        {
            return document.DocumentNode.Descendants().Where(n => n.GetAttributeValue("name", "") == name);
        }

    }
}
