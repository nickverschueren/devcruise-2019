using DevCruise.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DevCruise
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(o => o.Providers.Add<GzipCompressionProvider>()); 
            services.AddControllers();      
            services.AddDbContext<DevCruiseDbContext>(o => o.UseSqlite("Data Source=App_Data/DevCruiseDb.sqlite;"));
            services.AddSwaggerGen(c => c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "DevCruise API Documentation", Version = "1.0" }));
        }

        public void Configure(IApplicationBuilder app)
        {           
            //app.Run(context => context.Response.WriteAsync("Hello World!"));
            app.UseResponseCompression();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "DevCruise API v1.0"));
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}