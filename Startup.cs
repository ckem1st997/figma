using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using figma.Data;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using figma.CustomHandler;

using figma.DAL;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Http.Features;
using figma.Interface;
using figma.OutFile;
using figma.Models;
using figma.Services;

namespace figma
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailer, Mailer>();
            //  services.Configure<Smtp>(Configuration.GetSection("Smtp"));
            services.Configure<Smtp>(Configuration);
            services.AddControllers();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddMvc();
            services.AddDbContext<ShopProductContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("ShopProductContext")));
            //
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
     {

         //  config.Cookie.Name = "UserLoginCookie"; // Name of cookie     
         config.LoginPath = "/Home/Login"; // Path for the redirect to user login page    
         config.AccessDeniedPath = "/Csm/UserAccessDenied";
         // sau 10s sẽ tự out
         config.ExpireTimeSpan = TimeSpan.FromHours(1);
         config.Cookie.HttpOnly = true;
         config.Cookie.IsEssential = true;
     });
            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    // nếu tồi tại 2 kiểu được nhập vào
                    policyBuilder.UserRequireCustomClaim("UserName");
                    //  policyBuilder.UserRequireCustomClaim(ClaimTypes.Role);
                });
            });

            services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                //out sau ? s
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            services.AddHttpContextAccessor();
            services.Configure<PasswordHasherOptions>(option =>
            {
                option.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                option.IterationCount = 12000;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(GenericRepository<>));
            services.AddScoped<UnitOfWork>();
            services.AddRazorPages();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //if (env.IsDevelopment())
                //{
                //    app.UseDeveloperExceptionPage();
                //    app.Run(async (context) =>
                //    {
                //        var sb = new StringBuilder();
                //        var nl = System.Environment.NewLine;
                //        var rule = string.Concat(nl, new string('-', 40), nl);
                //        var authSchemeProvider = app.ApplicationServices
                //            .GetRequiredService<IAuthenticationSchemeProvider>();

                //        sb.Append($"Request{rule}");
                //        sb.Append($"{DateTimeOffset.Now}{nl}");
                //        sb.Append($"{context.Request.Method} {context.Request.Path}{nl}");
                //        sb.Append($"Scheme: {context.Request.Scheme}{nl}");
                //        sb.Append($"Host: {context.Request.Headers["Host"]}{nl}");
                //        sb.Append($"PathBase: {context.Request.PathBase.Value}{nl}");
                //        sb.Append($"Path: {context.Request.Path.Value}{nl}");
                //        sb.Append($"Query: {context.Request.QueryString.Value}{nl}{nl}");

                //        sb.Append($"Connection{rule}");
                //        sb.Append($"RemoteIp: {context.Connection.RemoteIpAddress}{nl}");
                //        sb.Append($"RemotePort: {context.Connection.RemotePort}{nl}");
                //        sb.Append($"LocalIp: {context.Connection.LocalIpAddress}{nl}");
                //        sb.Append($"LocalPort: {context.Connection.LocalPort}{nl}");
                //        sb.Append($"ClientCert: {context.Connection.ClientCertificate}{nl}{nl}");

                //        sb.Append($"Identity{rule}");
                //        sb.Append($"User: {context.User.Identity.Name}{nl}");
                //        var scheme = await authSchemeProvider
                //            .GetSchemeAsync(IISDefaults.AuthenticationScheme);
                //        sb.Append($"DisplayName: {scheme?.DisplayName}{nl}{nl}");

                //        sb.Append($"Headers{rule}");
                //        foreach (var header in context.Request.Headers)
                //        {
                //            sb.Append($"{header.Key}: {header.Value}{nl}");
                //        }
                //        sb.Append(nl);

                //        sb.Append($"Websockets{rule}");
                //        if (context.Features.Get<IHttpUpgradeFeature>() != null)
                //        {
                //            sb.Append($"Status: Enabled{nl}{nl}");
                //        }
                //        else
                //        {
                //            sb.Append($"Status: Disabled{nl}{nl}");
                //        }

                //        sb.Append($"Configuration{rule}");
                //        foreach (var pair in config.AsEnumerable())
                //        {
                //            sb.Append($"{pair.Key}: {pair.Value}{nl}");
                //        }
                //        sb.Append(nl);

                //        sb.Append($"Environment Variables{rule}");
                //        var vars = System.Environment.GetEnvironmentVariables();
                //        foreach (var key in vars.Keys.Cast<string>().OrderBy(key => key,
                //            StringComparer.OrdinalIgnoreCase))
                //        {
                //            var value = vars[key];
                //            sb.Append($"{key}: {value}{nl}");
                //        }

                //        context.Response.ContentType = "text/plain";
                //        await context.Response.WriteAsync(sb.ToString());
                //    });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
