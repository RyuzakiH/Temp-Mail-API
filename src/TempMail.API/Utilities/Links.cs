using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TempMail.API.Utilities
{
    internal class Links
    {
        internal static Uri CreateSourceLink(string id) => new Uri($"https://temp-mail.org/en/source/{id}/");

        internal static string GetId(Uri link) =>
            Regex.Match(link.ToString(), @"https://temp-mail.org/en/.*?/(?<id>.*)").Groups["id"].Value;
    }
}
