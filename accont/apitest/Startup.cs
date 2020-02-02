using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.OpenApi.Models;
using apitest.Modules;
using apitest.Interfaces;
using apitest.Services;

namespace apitest
{
    public class Startup
    {

        private IHostingEnvironment _env;
        private string  _connection;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            _connection = @"Data Source =DESKTOP-HG2J4TV\SQLEXPRESS;Initial Catalog=test4;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        }


        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<DBCommonContext>(options => options.UseSqlServer(_connection));
            services.AddScoped<IAccountInterface, AccountService>();
            services.AddScoped<IResultStatus, ResultStatus>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Test API",
                    Version = "v1"
                });
               
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
           .AddJsonOptions(options => {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }


        public void Configure(IApplicationBuilder app)
        {
           
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI Page");
                   
                });
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
