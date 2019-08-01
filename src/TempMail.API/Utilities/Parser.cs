using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TempMail.API.Utilities
{
    internal class Parser
    {
        internal static List<string> ExtractAvailableDomains(string document) =>
            Regex.Matches(document, @"<option value=""(?<domain>@.*?)"">.*?</option>").Cast<Match>()
                .Select(m => m.Groups["domain"].Value).ToList();

        internal static string ExtractEmail(string document) =>
            Regex.Match(document, @"<input id=""mail"".*?value=""(?<mail>.*?)"".*?/>").Groups["mail"].Value;

        internal static string ExtractEmailFromDeleteResponse(string response) =>
            Regex.Match(response, @"{""mail""\s*:\s*""(?<mail>.*?)""}").Groups["mail"].Value;

        internal static IEnumerable<string> ExtractMailsIds(string document) =>
            Regex.Matches(document, @"<a href=""https://temp-mail.org/en/.*?/(?<id>.*)"" class=""link"">").Cast<Match>()
                .Select(m => m.Groups["id"].Value);
    }
}
