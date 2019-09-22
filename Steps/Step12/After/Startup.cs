using System;
using System.Collections.Generic;
using AutoMapper;
using DevCruise.Model;
using DevCruise.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "DevCruise API Documentation", Version = "1.0" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://login.microsoftonline.com/0b53d2c1-bc55-4ab3-a161-927d289257f2/oauth2/v2.0/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { Scopes.AadReadAccess, "Access read operations" },
                                { Scopes.AadWriteAccess, "Access write operations" }
                            }
                        }
                    }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddSingleton<IConfigurationProvider>(s => new MapperConfiguration(c => c.AddMaps(typeof(Startup))));
            services.AddScoped(s => s.GetService<IConfigurationProvider>().CreateMapper(s.GetService));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.Authority = "https://login.microsoftonline.com/0b53d2c1-bc55-4ab3-a161-927d289257f2/v2.0";
                o.Audience = "4be39034-c55a-4ab3-bd3a-38fa0664bb53";
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy(Scopes.ReadAccess, builder => builder.RequireAssertion(context => context.User.HasScope(Scopes.ReadAccess)));
                o.AddPolicy(Scopes.WriteAccess, builder => builder.RequireAssertion(context => context.User.HasScope(Scopes.WriteAccess)));
            });
        }

        public void Configure(IApplicationBuilder app)
        {           
            //app.Run(context => context.Response.WriteAsync("Hello World!"));
            app.UseResponseCompression();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "DevCruise API v1.0");
                c.OAuthClientId("4be39034-c55a-4ab3-bd3a-38fa0664bb53");
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }
}