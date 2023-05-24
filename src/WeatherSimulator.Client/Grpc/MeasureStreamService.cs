using Grpc.Core;
using Microsoft.Extensions.Options;
using WeatherSimulator.Client.Configuration;
using WeatherSimulator.Client.Storages;
using WeatherSimulator.Proto;

namespace WeatherSimulator.Client.Grpc;

public class MeasureStreamService : IMeasureStreamService
{
    private readonly ILogger<MeasureService> _logger;
    private readonly WeatherSimulatorService.WeatherSimulatorServiceClient _client;
    private readonly ISensorStorage _sensorStorage;
    private AsyncDuplexStreamingCall<ToServerMessage, SensorData> _stream;
    private MeasureServiceConfig _config;

    private HashSet<string> _subscribeIds = new();

    public MeasureStreamService(WeatherSimulatorService.WeatherSimulatorServiceClient client,
        ISensorStorage sensorStorage, IOptionsMonitor<MeasureServiceConfig> config, ILogger<MeasureService> logger)
    {
        _client = client.WithHost(config.CurrentValue.GrpcUrl);
        _sensorStorage = sensorStorage;
        _logger = logger;
        _stream = _client.GetSensorsStream();
        _config = config.CurrentValue;
        config.OnChange(async (o) => await InitializeSensors(o));
    }
    
    public async Task ReadStream(CancellationToken stoppingToken)
    {
        _stream = _client.GetSensorsStream(cancellationToken: stoppingToken);
        await InitializeSensors(_config);

        await ReadResponseAsync(stoppingToken);
    }

    private async Task InitializeSensors(MeasureServiceConfig options)
    {
        var currentConfigSet = options.SensorIds.ToHashSet();
        
        var unsubscribeIdList = _subscribeIds.Except(currentConfigSet).ToArray();
        var subscribeIdList = currentConfigSet.Except(_subscribeIds).ToArray();

        await WriteRequestAsync(subscribeIdList, unsubscribeIdList);
        
        _config = options;
        _subscribeIds = currentConfigSet;
    }
    
    private async Task ReadResponseAsync(CancellationToken stoppingToken)
    {
        await foreach (var response in _stream.ResponseStream.ReadAllAsync(stoppingToken))
        {
            _sensorStorage.AddMeasureSensor(response);
        }
    }

    private async Task WriteRequestAsync(string[] subscribeIdList, string[] unsubscribeIdList)
    {
        await _stream.RequestStream.WriteAsync(new ToServerMessage()
        {
            SubscribeSensorsIds = {subscribeIdList},
            UnsubscribeSensorsIds = {unsubscribeIdList}
        });
    }
}