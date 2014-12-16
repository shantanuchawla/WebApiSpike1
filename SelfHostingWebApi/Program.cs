using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostingWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            Console.WriteLine("Starting WebApi Self host container...");

            // Start OWIN host 
            WebApp.Start<StartupConfiguration>(baseAddress);

            Console.WriteLine("Started WebApi Self host container.Press any key to stop the container...");

            Console.ReadLine(); 
        }
    }
}
