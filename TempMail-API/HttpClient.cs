using System;
using System.Net;

public class HttpClient : WebClient
{
    public CookieContainer CookieContainer { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool AllowAutoRedirect { get; set; }
    public bool KeepAlive { get; set; }
    public int Timeout { get; set; }
    public WebHeaderCollection DefaultHeaders { get; set; }
    public Uri RequestUri { get; private set; }
    public Uri ResponseUri { get; private set; }

    public HttpClient()
    {
        CookieContainer = new CookieContainer();
        AllowAutoRedirect = true;
        KeepAlive = true;
        Timeout = 30000;
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
        if (DefaultHeaders != null)
        {
            foreach (string item in DefaultHeaders)
                this.Headers.Set(item, DefaultHeaders[item]);
        }

        var request = (HttpWebRequest)base.GetWebRequest(address);
        request.CookieContainer = this.CookieContainer;
        request.KeepAlive = KeepAlive;
        request.AllowAutoRedirect = AllowAutoRedirect;
        request.Timeout = Timeout;
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        request.ServicePoint.Expect100Continue = false;

        return request;
    }

    protected override WebResponse GetWebResponse(WebRequest request)
    {
        WebResponse Response = null;
        try { Response = request.GetResponse(); }
        catch (WebException wb) { Response = ((HttpWebResponse)wb.Response); }

        if (Response != null)
        {
            this.StatusCode = ((HttpWebResponse)Response).StatusCode;
            this.RequestUri = request.RequestUri;
            this.ResponseUri = Response.ResponseUri;
        }

        return Response;
    }

}
