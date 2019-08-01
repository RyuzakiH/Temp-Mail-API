using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TempMail.API.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a string in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static string GetString(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.GetStringAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a string in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static string GetString(this HttpClient client, Uri requestUri)
        {
            return Task.Run(() => client.GetStringAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send an HTTP request as a synchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="request"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
        {
            return Task.Run(() => client.SendAsync(request)).Result;
        }        

    }
}
