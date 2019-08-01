using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TempMail.API.Extensions
{
    internal static class HttpContentExtensions
    {
        /// <summary>
        ///     Serialize the HTTP content to a string as a synchronous operation.
        /// </summary>
        internal static string ReadAsString(this HttpContent content)
        {
            return Task.Run(() => content.ReadAsStringAsync()).Result;
        }

        /// <summary>
        ///     Serialize the HTTP content to a string as a synchronous operation.
        /// </summary>
        internal static string ReadAsString(this HttpContent content, Encoding encoding)
        {
            return encoding.GetString(Task.Run(() => content.ReadAsByteArrayAsync()).Result);
        }

        /// <summary>
        ///     Serialize the HTTP content to a string as an asynchronous operation.
        /// </summary>
        internal async static Task<string> ReadAsStringAsync(this HttpContent content, Encoding encoding)
        {
            var bytes = await content.ReadAsByteArrayAsync();
            return encoding.GetString(bytes);
        }

    }
}
