using Bussiness.Flight;
using Bussiness.GetRoute;
using Bussiness.Journey;
using Bussiness.JourneyFlight;
using Bussiness.ServiceConsumptionLogic;
using Bussiness.Transport;
using Data_Access.Context;
using Data_Access.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API_NewShore
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
            services.AddCors();

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options
                        => options.UseSqlServer(Configuration.GetConnectionString("newshore")), ServiceLifetime.Transient);

            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.AddSwaggerGen();

            //call for services
            services.AddTransient<IServiceConsumption, ServiceConsumptionLogic>();
            services.AddTransient<IGetRoute, GetRoute>();
            services.AddTransient<IFlight, Flight>();
            services.AddTransient<IFlight, Flight>();
            services.AddTransient<IJourney, Journey>();
            services.AddTransient<ITransport, Transport>();
            services.AddTransient<IJourneyFlight, JourneyFlight>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_NewShore");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(options =>
            {
                options.WithOrigins("*");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
