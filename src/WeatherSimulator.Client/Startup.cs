using Microsoft.Extensions.Options;
using WeatherSimulator.Client.Configuration;
using WeatherSimulator.Client.Extensions;
using WeatherSimulator.Proto;

namespace WeatherSimulator.Client;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions(Configuration);
        services.AddServices();
        services.AddGrpcClient<WeatherSimulatorService.WeatherSimulatorServiceClient>(x =>
        {
            x.Address = new Uri(Configuration["WeatherServiceConfig:GrpcUrl"]);
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLogging();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}