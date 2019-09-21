using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace DevCruise
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(o => o.Providers.Add<GzipCompressionProvider>()); 
            services.AddControllers();      
        }

        public void Configure(IApplicationBuilder app)
        {           
            //app.Run(context => context.Response.WriteAsync("Hello World!"));
            app.UseResponseCompression();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}