using WeatherSimulator.Proto;

namespace WeatherSimulator.Client.Grpc;

public interface IMeasureService
{
    public Task<SensorData?> GetSensorDataByIdAsync(string sensorId, CancellationToken stoppingToken);
}