using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TempMail.API.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Result { get; set; }

    }
}
