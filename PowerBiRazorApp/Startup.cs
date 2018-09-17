using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerBiRazorApp.Authentication.AuthenticationHandler;
using PowerBiRazorApp.DataAccess;
namespace PowerBiRazorApp

{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var azureConfig = Configuration.GetSection("AzureAd");
            services.Configure<AzureAdSettings>(azureConfig);

            var powerbiConfig = Configuration.GetSection("PowerBi");
            services.Configure<PowerBiSettings>(powerbiConfig);

            services.AddScoped<AuthenticationHandler>();
            services.AddScoped<ReportRepository>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }

    public class AzureAdSettings
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string CallbackPath { get; set; }
        public string TokenType { get; set; }
    }

    public class PowerBiSettings
    {
        public string MainAddress { get; set; }
        public string ResourceAddress { get; set; }
        public string MasterUser { get; set; }
        public string MasterKey { get; set; }
        public string GroupId { get; set; }
    }
}
