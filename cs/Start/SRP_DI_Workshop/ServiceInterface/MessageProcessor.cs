using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace SRP_DI_Workshop.ServiceInterface
{
    public class MessageProcessor : OwinMiddleware
    {
        private readonly OrderProcessor _processor = new OrderProcessor();

        public MessageProcessor(OwinMiddleware next)
            : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            return Task.Run(() => ProcessMessage(context));
        }

        public void ProcessMessage(IOwinContext context)
        {
            JsonSerializer serializer = new JsonSerializer();
            string requestBody;

            using (StreamReader sr = new StreamReader(context.Request.Body))
                requestBody = sr.ReadToEnd();

            RequestMessage reqMsg;

            using (JsonReader sr = new JsonTextReader(new StringReader(requestBody)))
            {
                try
                {
                    reqMsg = serializer.Deserialize<RequestMessage>(sr);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception while trying to deserialize message: " + ex, "ParseError");
                    WriteJsonResponse(context, serializer, HttpStatusCode.BadRequest, requestBody);
                    return;
                }
            }

            ResponseMessage response = _processor.ProcessOrder(reqMsg);

            WriteJsonResponse(context, serializer, response.StatusCode, response.ResponseBody);
        }

        private static void WriteJsonResponse(IOwinContext context, JsonSerializer serializer, HttpStatusCode statusCode, object message)
        {
            context.Response.StatusCode = (int)statusCode;

            if (message != null)
            {
                context.Response.Headers.Append("Content-Type", "application/json");

                using (StreamWriter sw = new StreamWriter(context.Response.Body, Encoding.UTF8))
                    serializer.Serialize(sw, message);
            }
        }
    }
}
