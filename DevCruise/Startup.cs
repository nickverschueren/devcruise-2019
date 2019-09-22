using System;
using System.Collections.Generic;
using AutoMapper;
using Euricom.DevCruise.Extensions;
using Euricom.DevCruise.Model;
using Euricom.DevCruise.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Euricom.DevCruise
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public IApiVersionDescriptionProvider ApiVersionDescriptionProvider { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(o => o.InputFormatters.RemoveType<SystemTextJsonInputFormatter>())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.UseCamelCasing(false);
                });

            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddResponseCompression(o =>
            {
                o.Providers.Add<GzipCompressionProvider>();
                o.Providers.Add<BrotliCompressionProvider>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = "https://login.microsoftonline.com/0b53d2c1-bc55-4ab3-a161-927d289257f2/v2.0";
                    o.Audience = "4be39034-c55a-4ab3-bd3a-38fa0664bb53";
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy(Scopes.ReadAccess, builder => builder.RequireAssertion(context => context.User.HasScope(Scopes.ReadAccess)));
                o.AddPolicy(Scopes.WriteAccess, builder => builder.RequireAssertion(context => context.User.HasScope(Scopes.WriteAccess)));
            });

            services.AddDbContext<DevCruiseDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DevCruiseDb").FormatAppDataPath(WebHostEnvironment.ContentRootPath));
            });

            services.AddSwaggerGen(c =>
            {
                foreach (var description in ApiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "DevCruise API",
                        Version = description.GroupName.ToLower(),
                        Description = description.IsDeprecated ? "Deprecated" : ""
                    });
                }

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

            services.AddSingleton<AutoMapper.IConfigurationProvider>(s => new MapperConfiguration(c => c.AddMaps(typeof(Startup))));
            services.AddScoped(s => s.GetService<AutoMapper.IConfigurationProvider>().CreateMapper(s.GetService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ApiVersionDescriptionProvider = provider;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseResponseCompression();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("4be39034-c55a-4ab3-bd3a-38fa0664bb53");
                c.OAuthAppName("DevCruise API SwaggerUI");

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"DevCruise API {description.GroupName.ToUpperInvariant()}{(description.IsDeprecated ? " (DEPRECATED)" : "")}");
                }
            });

            app.UseRouting();

            // Must be used AFTER Routing middleware!
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}