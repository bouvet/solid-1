using System;
using Microsoft.Owin.Hosting;
using Owin;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.WriteLine("MessageProcessor listening on http://localhost:12345");
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof (MessageProcessor));
        }
    }
}
