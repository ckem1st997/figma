﻿using System;
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
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using figma.Hubs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.IO;

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
            services.AddImageSharp(options =>
            {
                // You only need to set the options you want to change here
                // All properties have been listed for demonstration purposes
                // only.
                options.MemoryStreamManager = new RecyclableMemoryStreamManager();
                options.BrowserMaxAge = TimeSpan.FromDays(7);
                options.CacheMaxAge = TimeSpan.FromDays(365);
                options.CachedNameLength = 8;
                options.OnParseCommandsAsync = _ => Task.CompletedTask;
                options.OnBeforeSaveAsync = _ => Task.CompletedTask;
                options.OnProcessedAsync = _ => Task.CompletedTask;
                options.OnPrepareResponseAsync = _ => Task.CompletedTask;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSignalR();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddTransient<IMailer, Mailer>();
            services.Configure<Smtp>(Configuration.GetSection("Smtp"));
            services.Configure<VNPAY>(Configuration.GetSection("VNPAY"));
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
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
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
            //
            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(Configuration.GetConnectionString("ShopProductContext"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            // Google Login

            services.AddAuthentication(
                options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
                        .AddFacebook(facebookOptions =>
                        {
                            facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                            facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                        })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, ILoggerFactory loggerFactor)
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
            app.UseImageSharp();
            var loggingOptions = this.Configuration.GetSection("Log4NetCore")
                                              .Get<Log4NetProviderOptions>();
            loggerFactor.AddLog4Net(loggingOptions);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseSession();
            app.UseHangfireDashboard();
            //thực hiện 1 lần và ngay lập tức
            //  backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }

    }
}

//docker inspect –format='{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' 8fd29e4c4919
//172.17.93.74
//172.17.89.1  docker run -d -p 1433:1433 -e sa_password= -e ACCEPT_EULA=Y microsoft/mssql-server-windows
//docker run -d -p 1433:1433 -e sa_password=12345678h -eACCEPT_EULA=Y microsoft/mssql-server-windows-express
//  "ShopProductContext": "Data Source=DESKTOP-TPOA8KJ;Initial Catalog=sh3;Integrated Security=True"
//"ShopProductContext": "Server=host.docker.internal,1433;Initial Catalog=sh3;User ID=aspnet;Password=h12345678;Integrated Security=false"
//6LdnUNgZAAAAABU3v7itGiAPvEBhux78ViWF8yQ6
//6LdnUNgZAAAAAOppZRLZo8_l5KzlEkEm3wnLHfXx
