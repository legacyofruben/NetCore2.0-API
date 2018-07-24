using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_JabilBot.Models;
using API_JabilBot.Models.Contexts;
using API_JabilBot.Services;
using API_JabilBot.Services.Interfaces;
using API_JabilBot.Services.Interfaces.ServiceNow;
using API_JabilBot.Services.ServiceNow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API_JabilBot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //var environment = Configuration["AppSettings:Environment"];
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ItemContext>(opt =>
                    opt.UseInMemoryDatabase("ItemList"));
            services.AddDbContext<EmployeeContext>(opt =>
                    opt.UseInMemoryDatabase("EmployeeList"));
            services.AddMvc();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddResponseCaching();                                            // ResponseCaching
            services.AddScoped<IChangeRequestService, ChangeRequestService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IQnAService, QnAService>();
            services.AddScoped<IGeneralFunctionsService, GeneralFunctionsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCaching();
            app.UseMvc();
        }
    }
}
