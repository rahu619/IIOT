using IIOT.MessageConsumer;
using IIOT.MessageConsumer.Configuration;
using IIOT.MessageConsumer.Data;
using IIOT.MessageConsumer.Data.Repositories;
using IIOT.MessageConsumer.Service;
using IIOT.MessageConsumer.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace IIOT.API
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IIOT.API", Version = "v1" });
            });

            services.AddHostedService<ConsumerService>();
            services.AddScoped<ITemperatureRepository, TemperatureRepository>();
            services.AddScoped<ITemperatureService, TemperatureService>();
            services.Configure<ConsumerConfiguration>(Configuration.GetSection(nameof(ConsumerConfiguration)));
            services.AddEntityFrameworkSqlite().AddDbContext<MessageConsumerDbContext>();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IIOT.API v1"));
            }

            app.UseRouting();
            app.UseCors("default");
            app.UseEndpoints(endpoints =>
            endpoints.MapHub<TemperatureHub>("/temperaturehub")
                 .RequireCors((policyBuilder) => policyBuilder
                 .WithOrigins("http://127.0.0.1:5500")
                 .SetIsOriginAllowed(_ => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials())
                 );


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

        }
    }
}
