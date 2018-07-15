using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mehrsan.Core.Web.Data;
using Mehrsan.Dal.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mehrsan.Core.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Common.Common.Configuration = (ConfigurationRoot)builder.Build();
            Common.Common.Initialise();

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add framework services.
            services.AddMvc();

            //Add database services.
            var connectionString = Common.Common.Configuration.GetConnectionString("WordEntities");

            services.AddDbContext<WordEntities>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Common.Common.Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }


            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Word}/{action=Index}/{id?}");
            });
        }
    }
}
