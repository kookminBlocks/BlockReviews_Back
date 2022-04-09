using BLockReviewsAPI.Bootstrapper;
using BLockReviewsAPI.DBService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(opt =>
            //{
            //    opt.AddDefaultPolicy(
            //       policy =>
            //       {
            //           policy.WithOrigins("http://localhost:3000");
            //       });
            //});            

            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddHttpClient();

            services.AddHttpClient("BlockReview", httpClient =>
            {
                httpClient.BaseAddress = new Uri("http://3.38.183.241/");
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                //httpClient.DefaultRequestHeaders.Add(
                //    HeaderNames.Accept, "application/vnd.github.v3+json");
                //httpClient.DefaultRequestHeaders.Add(
                //    HeaderNames.UserAgent, "HttpRequestsSample");
            });

            services.AddControllers();

            services.AddPoemloConfig(Configuration);

            services.AddSwaggerConfiguration();

            services.AddEtherBlock();            

            services.AddDBservices();

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(
                   policy =>
                   {
                       policy.WithOrigins("https://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                       policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                   });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerSetup();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "/swagger");
            });
        }
    }
}
