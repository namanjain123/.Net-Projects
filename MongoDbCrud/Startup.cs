using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace MongoDbCrud
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
            //Enable Cors setup
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            //Json Serilaizer setup
            services.AddControllersWithViews().AddNewtonsoftJson(
                a=>a.SerializerSettings.ReferenceLoopHandling
                =Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .AddNewtonsoftJson(
                b=>
                b.SerializerSettings.ContractResolver=new DefaultContractResolver()
                );
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Configure corps to allow in configuratiion
            app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //Addition of photos to be used
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(Directory.GetCurrentDirectory(),"Photo")),
                RequestPath = "/Photo"
            });
        }
    }
}
