using HtmlAgilityPack;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        #region Get

        /// <summary>
        ///     Send a GET request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.GetAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, Uri requestUri)
        {
            return Task.Run(() => client.GetAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, string requestUri, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.GetAsync(requestUri, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with an HTTP completion option as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">
        ///     An HTTP completion option value that indicates when the operation should be considered completed.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, string requestUri, HttpCompletionOption completionOption)
        {
            return Task.Run(() => client.GetAsync(requestUri, completionOption)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, Uri requestUri, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.GetAsync(requestUri, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with an HTTP completion option as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">
        ///     An HTTP completion option value that indicates when the operation should be considered completed.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption)
        {
            return Task.Run(() => client.GetAsync(requestUri, completionOption)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with an HTTP completion option and a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">
        ///     An HTTP completion option value that indicates when the operation should be considered completed.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.GetAsync(requestUri, completionOption, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri with an HTTP completion option and a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">
        ///     An HTTP completion option value that indicates when the operation should be considered completed.
        /// </param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Get(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.GetAsync(requestUri, completionOption, cancellationToken)).Result;
        }

        #endregion



        #region GetString

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

        #endregion

        #region GetByteArray

        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a byte array in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static byte[] GetByteArray(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.GetByteArrayAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a byte array in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static byte[] GetByteArray(this HttpClient client, Uri requestUri)
        {
            return Task.Run(() => client.GetByteArrayAsync(requestUri)).Result;
        }

        #endregion

        #region GetStream

        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a stream in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static Stream GetStream(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.GetStreamAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a GET request to the specified Uri and return the response body as a stream in a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static Stream GetStream(this HttpClient client, Uri requestUri)
        {
            return Task.Run(() => client.GetStreamAsync(requestUri)).Result;
        }

        #endregion


        #region GetHtmlDocument

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies and returns the HtmlDocument of the result.
        /// </summary>
        public static HtmlDocument GetHtmlDocument(this HttpClient client, string url)
        {
            var document = new HtmlDocument();
            document.LoadHtml(client.GetString(url));

            return document;
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies and returns the HtmlDocument of the result.
        /// </summary>
        public static HtmlDocument GetHtmlDocument(this HttpClient client, string url, Encoding encoding)
        {
            var response = client.Get(url);

            var document = new HtmlDocument();
            document.LoadHtml(response.Content.ReadAsString(encoding));

            return document;
        }


        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> HtmlDocument of the result </returns>
        public static async Task<HtmlDocument> GetHtmlDocumentAsync(this HttpClient client, string url)
        {
            var document = new HtmlDocument();
            await Task.Run(async () => document.LoadHtml(await client.GetStringAsync(url)));

            return document;
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> HtmlDocument of the result </returns>
        public static async Task<HtmlDocument> GetHtmlDocumentAsync(this HttpClient client, string url, Encoding encoding)
        {
            var response = await client.GetAsync(url);

            var document = new HtmlDocument();
            await Task.Run(async () => document.LoadHtml(await response.Content.ReadAsStringAsync(encoding)));

            return document;
        }

        #endregion



        #region GetJsonObject

        public static T GetJsonObject<T>(this HttpClient client, string url)
        {
            return JsonConvert.DeserializeObject<T>(client.Get(url).Content.ReadAsString());
        }

        public static T GetJsonObject<T>(this HttpClient client, string url, Encoding encoding)
        {
            return JsonConvert.DeserializeObject<T>(client.Get(url).Content.ReadAsString(encoding));
        }

        public static async Task<T> GetJsonObjectAsync<T>(this HttpClient client, string url)
        {
            return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await (await client.GetAsync(url)).Content.ReadAsStringAsync()));
        }

        public static async Task<T> GetJsonObjectAsync<T>(this HttpClient client, string url, Encoding encoding)
        {
            return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await (await client.GetAsync(url)).Content.ReadAsStringAsync(encoding)));
        }

        #endregion







        #region Post

        /// <summary>
        ///     Send a POST request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Post(this HttpClient client, string requestUri, HttpContent content)
        {
            return Task.Run(() => client.PostAsync(requestUri, content)).Result;
        }

        /// <summary>
        ///     Send a POST request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Post(this HttpClient client, Uri requestUri, HttpContent content)
        {
            return Task.Run(() => client.PostAsync(requestUri, content)).Result;
        }

        /// <summary>
        ///     Send a POST request with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Post(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.PostAsync(requestUri, content, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a POST request with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Post(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.PostAsync(requestUri, content, cancellationToken)).Result;
        }

        #endregion



        #region Put

        /// <summary>
        ///     Send a PUT request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Put(this HttpClient client, string requestUri, HttpContent content)
        {
            return Task.Run(() => client.PostAsync(requestUri, content)).Result;
        }

        /// <summary>
        ///     Send a PUT request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Put(this HttpClient client, Uri requestUri, HttpContent content)
        {
            return Task.Run(() => client.PostAsync(requestUri, content)).Result;
        }

        /// <summary>
        ///     Send a PUT request with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="content">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Put(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.PostAsync(requestUri, content, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a PUT request with a cancellation token as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="content">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        public static HttpResponseMessage Put(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.PostAsync(requestUri, content, cancellationToken)).Result;
        }

        #endregion



        #region Delete

        /// <summary>
        ///     Send a DELETE request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the synchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Delete(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.DeleteAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a DELETE request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the synchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Delete(this HttpClient client, Uri requestUri)
        {
            return Task.Run(() => client.DeleteAsync(requestUri)).Result;
        }

        /// <summary>
        ///     Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the synchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Delete(this HttpClient client, string requestUri, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.DeleteAsync(requestUri, cancellationToken)).Result;
        }

        /// <summary>
        ///     Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        ///     Returns System.Threading.Tasks.Task`1.The task object representing the synchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="requestUri"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Delete(this HttpClient client, Uri requestUri, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.DeleteAsync(requestUri, cancellationToken)).Result;
        }

        #endregion



        #region Send

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

        /// <summary>
        ///     Send an HTTP request as a synchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="completionOption">
        ///     When the operation should complete (as soon as a response is available or after reading the whole response content).
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="request"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return Task.Run(() => client.SendAsync(request, completionOption)).Result;
        }

        /// <summary>
        ///     Send an HTTP request as a synchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="completionOption">
        ///     When the operation should complete (as soon as a response is available or after reading the whole response content).
        /// </param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="request"/> was null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The request message was already sent by the System.Net.Http.HttpClient instance.
        /// </exception>
        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return Task.Run(() => client.SendAsync(request, completionOption, cancellationToken)).Result;
        }


        #endregion









        #region HttpContent

        /// <summary>
        ///     Serialize the HTTP content into a stream of bytes and copies it to the stream
        ///      object provided as the stream parameter.
        /// </summary>
        /// <param name="stream">The target stream.</param>
        public static void CopyTo(this HttpContent content, Stream stream)
        {
            Task.Run(() => content.CopyToAsync(stream)).Wait();
        }

        /// <summary>
        ///     Serialize the HTTP content to a memory buffer as a synchronous operation.
        /// </summary>
        public static void LoadIntoBuffer(this HttpContent content)
        {
            Task.Run(() => content.LoadIntoBufferAsync()).Wait();
        }

        /// <summary>
        ///     Serialize the HTTP content to a memory buffer as a synchronous operation.
        /// </summary>
        /// <param name="maxBufferSize">The maximum size, in bytes, of the buffer to use.</param>
        public static void LoadIntoBuffer(this HttpContent content, long maxBufferSize)
        {
            Task.Run(() => content.LoadIntoBufferAsync(maxBufferSize)).Wait();
        }

        /// <summary>
        ///     Serialize the HTTP content to a byte array as a synchronous operation.
        /// </summary>
        public static byte[] ReadAsByteArray(this HttpContent content)
        {
            return Task.Run(() => content.ReadAsByteArrayAsync()).Result;
        }

        /// <summary>
        ///     Serialize the HTTP content and return a stream that represents the content as a synchronous operation.
        /// </summary>
        public static Stream ReadAsStream(this HttpContent content)
        {
            return Task.Run(() => content.ReadAsStreamAsync()).Result;
        }

        /// <summary>
        ///     Serialize the HTTP content to a string as a synchronous operation.
        /// </summary>
        public static string ReadAsString(this HttpContent content)
        {
            return Task.Run(() => content.ReadAsStringAsync()).Result;
        }

        /// <summary>
        ///     Serialize the HTTP content to a string as a synchronous operation.
        /// </summary>
        public static string ReadAsString(this HttpContent content, Encoding encoding)
        {
            return encoding.GetString(Task.Run(() => content.ReadAsByteArrayAsync()).Result);
        }

        /// <summary>
        ///     Serialize the HTTP content to a string as an asynchronous operation.
        /// </summary>
        public async static Task<string> ReadAsStringAsync(this HttpContent content, Encoding encoding)
        {
            var bytes = await content.ReadAsByteArrayAsync();
            return encoding.GetString(bytes);
        }



        public static T ReadAsJsonObject<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsString());
        }

        public static T ReadAsJsonObject<T>(this HttpContent content, Encoding encoding)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsString(encoding));
        }

        public static async Task<T> ReadAsJsonObjectAsync<T>(this HttpContent content)
        {
            return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync()));
        }

        public static async Task<T> ReadAsJsonObjectAsync<T>(this HttpContent content, Encoding encoding)
        {
            return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync(encoding)));
        }







        
        public static HtmlDocument ReadAsHtmlDocument(this HttpContent content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content.ReadAsString());        
            return document;
        }
        
        public static HtmlDocument ReadAsHtmlDocument(this HttpContent content, Encoding encoding)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content.ReadAsString(encoding));
            return document;
        }

        
        public static async Task<HtmlDocument> ReadAsHtmlDocumentAsync(this HttpContent content)
        {
            var document = new HtmlDocument();
            await Task.Run(async () => document.LoadHtml(await content.ReadAsStringAsync()));
            return document;
        }
        
        public static async Task<HtmlDocument> ReadAsHtmlDocumentAsync(this HttpContent content, Encoding encoding)
        {
            var document = new HtmlDocument();
            await Task.Run(async () => document.LoadHtml(await content.ReadAsStringAsync(encoding)));
            return document;
        }


        #endregion





        //public static T RunSync<T>(this Task<T> task)
        //{
        //    //return Task.Run(() => task).Result;

        //    task.RunSynchronously();

        //    return task.Result;
        //}



    }
}
