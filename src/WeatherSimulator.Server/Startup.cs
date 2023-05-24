using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherSimulator.Server.Configurations;
using WeatherSimulator.Server.GrpcServices;
using WeatherSimulator.Server.Services;
using WeatherSimulator.Server.Services.Abstractions;
using WeatherSimulator.Server.Services.Background;
using WeatherSimulator.Server.Storages;
using WeatherSimulator.Server.Storages.Abstractions;

namespace WeatherSimulator.Server;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServerHostedServices(Configuration);
        services.AddLogging();
        services.AddGrpc();
        services.AddGrpcReflection();
        services.AddSwaggerGen();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                opt.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<WeatherSimulatorService>();
            endpoints.MapGrpcReflectionService();
            endpoints.MapControllers();
        });
    }
}
