using System.Net;

namespace TempMail.API.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Result { get; set; }

    }
}
