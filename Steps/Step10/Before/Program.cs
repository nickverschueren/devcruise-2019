using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace DevCruise
{
    class Program
    {
        static void Main(string[] args)
        {
            var webRoot = Path.Combine(Environment.CurrentDirectory, "wwwroot");
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseWebRoot(webRoot);
                    webBuilder.UseStartup<Startup>();
                });
            var host = hostBuilder.Build();
            host.Run();
        }
    }
}
