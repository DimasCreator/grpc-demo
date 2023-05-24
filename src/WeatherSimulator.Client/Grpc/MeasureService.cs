using Microsoft.Extensions.Options;
using WeatherSimulator.Client.Configuration;
using WeatherSimulator.Proto;

namespace WeatherSimulator.Client.Grpc;

public class MeasureService : IMeasureService
{
    private readonly ILogger<MeasureService> _logger;
    private readonly WeatherSimulatorService.WeatherSimulatorServiceClient _client;

    public MeasureService(WeatherSimulatorService.WeatherSimulatorServiceClient client,
        IOptions<MeasureServiceConfig> config, ILogger<MeasureService> logger)
    {
        _client = client.WithHost(config.Value.GrpcUrl);
        _logger = logger;
    }

    public async Task<SensorData?> GetSensorDataByIdAsync(string sensorId, CancellationToken stoppingToken)
    {
        var sensorData = await _client.GetCurrentMeasureAsync(new SensorId {SensorId_ = sensorId},
            cancellationToken: stoppingToken);
        
        return sensorData;
    }
}