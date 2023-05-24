using WeatherSimulator.Proto;

namespace WeatherSimulator.Client.Storages;

public interface ISensorStorage
{
    public IEnumerable<SensorData> GetMeasuresBySensorId(Guid sensorId);
    public void AddMeasureSensor(SensorData sensorData);
}