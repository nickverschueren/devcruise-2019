using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace DevCruise
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.Configure(app => {
                        app.Run(context => 
                            context.Response.WriteAsync("Hello World!"));
                    });
                });
            var host = hostBuilder.Build();
            host.Run();
        }
    }
}
