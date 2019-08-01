using System;

namespace TempMail.API.Utilities
{
    internal static class Utils
    {
        internal static string NormalizeDomain(string domain) => (domain[0] == '@') ? domain : '@' + domain;

        internal static double GetJavascriptDate()
        {
            return Math.Round(DateTime.UtcNow
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds);
        }
    }
}
