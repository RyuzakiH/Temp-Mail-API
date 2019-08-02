using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TempMail.API.Constants;

namespace TempMail.API.Utilities
{
    internal class Parser
    {
        internal static List<string> GetAvailableDomains(string document) =>
            RegexPatterns.DomainsRegex.Matches(document).Cast<Match>().Select(m => m.Groups["domain"].Value).ToList();
        
        internal static string GetEmail(string response) => RegexPatterns.EmailRegex.Match(response).Groups["mail"].Value;

        internal static string GetMailId(Uri link) => RegexPatterns.MailLinkRegex.Match(link.ToString()).Groups["id"].Value;

        internal static IEnumerable<string> GetMailsIds(string document) =>
            RegexPatterns.MailIdsRegex.Matches(document).Cast<Match>().Select(m => m.Groups["id"].Value);
    }
}