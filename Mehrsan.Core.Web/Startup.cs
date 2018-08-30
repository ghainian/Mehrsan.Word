using System;
using System.Collections.Generic;
using Mehrsan.Dal.DB.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mehrsan.Core.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mehrsan.Dal.DB;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Mehrsan.Business.Interface;
using Mehrsan.Dal.DB.Interface;
using Mehrsan.Business;

namespace Mehrsan.Core.Web
{
    /// <summary>
    /// This is Startup class
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            InitialiseCommon();
        }

        private void InitialiseCommon()
        {
            var maxImg = Configuration["MaxImagePerWord"].ToString();
            var maxImagePerWord = int.Parse(Configuration["MaxImagePerWord"].ToString());
            var agentPath = Configuration["AgentPath"].ToString();
            var backupDir = Configuration["BackupDir"].ToString();
            var videoDirectory = Configuration["VideoDirectory"].ToString();
            var downloadGoogleImageWaitTime = int.Parse(Configuration["DownloadGoogleImageWaitTime"].ToString());
            var nofRelatedSentences = int.Parse(Configuration["NofRelatedSentences"].ToString());
            var logDirectory = Configuration["LogDirectory"].ToString();
            Common.Common.Initialise(maxImagePerWord, agentPath, backupDir,
            videoDirectory, downloadGoogleImageWaitTime, nofRelatedSentences, logDirectory);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("WordEntities");

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();
            services.AddTransient<IDAL, DAL>();
            services.AddTransient<IWordApis, WordApis>();
            services.AddTransient<IWordRepository, WordRepository>();
            services.AddTransient<IWordEntities, WordEntities>();
            services.AddTransient<Common.Interface.ILogger, Common.Logger>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/login");


            services
                .AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)

                .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Identity/Account/login";
                options.LogoutPath = "/Identity/Account/logout";
            });

            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminUser", policy =>
                          policy.RequireClaim("UserType", "Admin"));
            });



            services.AddDbContext<WordEntities>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    internal class ClaimsTransformer : IClaimsTransformation
    {
        // Can consume services from DI as needed, including scoped DbContexts
        public ClaimsTransformer(IHttpContextAccessor httpAccessor) { }
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal p)
        {
            p.AddIdentity(new ClaimsIdentity());
            return Task.FromResult(p);
        }
    }

}
