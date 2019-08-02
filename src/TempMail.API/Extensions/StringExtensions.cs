using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TempMail.API.Extensions
{
    internal static class StringExtensions
    {
        internal static readonly Encoding DefaultEncoding = Encoding.UTF8;

        internal static MemoryStream ToMemoryStream(this string source, [Optional]Encoding encoding) =>
            new MemoryStream((encoding ?? DefaultEncoding).GetBytes(source ?? ""));
    }
}
