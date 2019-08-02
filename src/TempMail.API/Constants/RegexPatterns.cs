using System.Text.RegularExpressions;

namespace TempMail.API.Constants
{
    internal class RegexPatterns
    {
        private const string MailLinkPattern = "https://temp-mail.org/en/.*?/(?<id>.*)";

        internal static readonly Regex EmailRegex = new Regex(@"{""mail""\s*:\s*""(?<mail>.*?)""}");

        internal static readonly Regex DomainsRegex = new Regex(@"<option value=""(?<domain>@.*?)"">.*?</option>");

        internal static readonly Regex MailIdsRegex = new Regex($@"<a href=""{MailLinkPattern}"" class=""link"">");

        internal static readonly Regex MailLinkRegex = new Regex(MailLinkPattern);


    }
}
