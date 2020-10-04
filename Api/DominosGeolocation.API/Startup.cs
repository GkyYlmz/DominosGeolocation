using System;
using System.IO;
using System.Reflection;
using DominosGeolocation.Data;
using DominosGeolocation.Data.Models;
using DominosGeolocation.Data.Services.ServiceOrder;
using DominosGeolocation.Data.Services.SourceDestination;
using DominosGeolocation.Logging;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

namespace DominosGeolocation.API
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
            services.AddDbContext<GeolocationContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings"]));

            services.AddScoped(typeof(IRepository<>), typeof(GeneralRepository<>));
            services.AddTransient(typeof(IOrderService), typeof(OrderService));
            services.AddTransient(typeof(IDestinationSourceService), typeof(DestinationSourceService));
            services.AddControllers();

           
            var path = "log4net.config";
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            Logger.Configure(repo, path);

           // Logger.DLog.Debug("asdasdasd");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    name: "v1",
                    new OpenApiInfo
                    {
                        Title = "GeolocationApi",
                        Description = "GeolocationApi",
                        Version = "v1",
                        TermsOfService = new Uri("https://www.dominos.com/tr"),
                        Contact = new OpenApiContact
                        {
                            Name = "Dominos Code Team",
                            Email = "DominosDestek@gmail.com",
                            Url = new Uri("https://www.dominos.com/tr")
                        }
                    }
                );
            });

            services.AddSwaggerGenWithConventionalRoutes();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                ConventionalRoutingSwaggerGen.UseRoutes(endpoints);
            });

            app.UseSwagger(sw =>
            {
                sw.RouteTemplate = "swagger/{documentName}/swagger.json";
            });


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: $"/swagger/v1/swagger.json", name: "Dominos.API");
                c.RoutePrefix = $"swagger";
            });

        }
    }
}
