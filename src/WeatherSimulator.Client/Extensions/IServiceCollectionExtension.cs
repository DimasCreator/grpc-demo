using WeatherSimulator.Client.Configuration;
using WeatherSimulator.Client.Grpc;
using WeatherSimulator.Client.Storages;
using WeatherSimulator.Client.Background;

namespace WeatherSimulator.Client.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RetryConfig>().BindConfiguration("RetryConfig");
        services.AddOptions<MeasureServiceConfig>().BindConfiguration("WeatherServiceConfig");
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMeasureService, MeasureService>();
        services.AddScoped<IMeasureStreamService, MeasureStreamService>();
        services.AddSingleton<ISensorStorage, SensorStorage>();
        services.AddHostedService<MeasuresUpdateService>();
    }
}