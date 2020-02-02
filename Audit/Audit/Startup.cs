using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Audit.services;
using Audit.interfaces;
using audit.db;


namespace Audit
{
    public class Startup
    {

        private IHostingEnvironment env = null;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBCommonContext>(options => options.UseSqlServer(@"Server=LS2;Database=Audit;user=sa;password=nhtcrf;"));
            services.AddDbContext<DBDictinaryContext>(options => options.UseSqlServer(@"Server=Ls2;Database=Spravochnik;user=spravochniksql;password=spravochniksql;"));
            
            services.AddHttpContextAccessor();
            services.AddTransient<IRegistryService, RegistryService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IHolyDaysService, HolyDaysService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddTransient<IReportsService, ReportsService>();
       
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {

                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "audit.auth";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromDays(9999);               
                options.SlidingExpiration = true;
               
            });

          

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = (env.IsDevelopment() ? "client" : "client/dist");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
           

            app.UseMvcWithDefaultRoute();

            app.UseSpa(spa =>
            {

                spa.Options.SourcePath = (env.IsDevelopment() ? "client" : "client/dist");
                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");

            });

          
        }
    }
}
