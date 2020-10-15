using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using figma.Interface;
using figma.Models;
using figma.Services;
using Microsoft.AspNetCore.Antiforgery;
using System.Text.Encodings.Web;
using System.Text.Unicode;

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
            services.AddMemoryCache();
            services.AddTransient<IMailer, Mailer>();
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
         config.AccessDeniedPath = "/Home/UserAccessDenied";
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
            services.AddSingleton<HtmlEncoder>(
     HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });
            services.AddRazorPages();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
