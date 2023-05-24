using Microsoft.Extensions.Options;
using WeatherSimulator.Client.Grpc;
using Polly;
using WeatherSimulator.Client.Configuration;

namespace WeatherSimulator.Client.Background;

public class MeasuresUpdateService : BackgroundService
{
    private readonly ILogger<MeasuresUpdateService> _logger;
    private readonly IOptionsMonitor<RetryConfig> _retryConfig;
    private readonly IServiceScopeFactory _factory;
    private int _minRetryDelaySeconds = 5;
    private int _maxRetryDelaySeconds = 10;

    public MeasuresUpdateService(ILogger<MeasuresUpdateService> logger,
        IOptionsMonitor<RetryConfig> retryConfig, IServiceScopeFactory factory)
    {
        _logger = logger;
        _retryConfig = retryConfig;
        _factory = factory;
        _retryConfig.OnChange(OnChangeRetryConfig);
    }

    private void OnChangeRetryConfig(RetryConfig options)
    {
        _minRetryDelaySeconds = _retryConfig.CurrentValue.MinRetryDelaySeconds;
        _maxRetryDelaySeconds = _retryConfig.CurrentValue.MaxRetryDelaySeconds;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rnd = new Random();
        await Policy.Handle<Exception>()
            .WaitAndRetryForeverAsync(i =>
                TimeSpan.FromSeconds(rnd.Next(_minRetryDelaySeconds, _maxRetryDelaySeconds)), RetryLog)
            .ExecuteAsync(() =>
                _factory.CreateScope().ServiceProvider.GetRequiredService<IMeasureStreamService>().ReadStream(stoppingToken));
    }
    
    private void RetryLog(Exception exception, TimeSpan timeSpan)
    {
        _logger.LogError(exception,
            "Next connection attempt in {RetryTimeSpan} seconds \n{ExceptionMessage}",
            timeSpan.TotalSeconds, exception.Message);
    }
}