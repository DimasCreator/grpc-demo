namespace WeatherSimulator.Client.Grpc;

public interface IMeasureStreamService
{
    public Task ReadStream(CancellationToken stoppingToken);
}