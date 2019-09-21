using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevCruise
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {       
        }

        public void Configure(IApplicationBuilder app)
        {           
            //app.Run(context => context.Response.WriteAsync("Hello World!"));
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}