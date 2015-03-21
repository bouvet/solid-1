using System;
using System.Net;

namespace SRP_DI_Workshop.ServiceInterface
{
    [Serializable]
    public class ResponseMessage
    {
        public HttpStatusCode StatusCode { get; set; }
        public object ResponseBody { get; set; }
    }
}