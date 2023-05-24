using System.Collections.Concurrent;
using WeatherSimulator.Proto;

namespace WeatherSimulator.Client.Storages;

public class SensorStorage : ISensorStorage
{
    private readonly ConcurrentDictionary<Guid, List<SensorData>> _storage;

    public SensorStorage()
    {
        _storage = new ConcurrentDictionary<Guid, List<SensorData>>();
    }

    public IEnumerable<SensorData> GetMeasuresBySensorId(Guid sensorId)
    {
        if (_storage.TryGetValue(sensorId, out var sensorData))
        {
            return sensorData;
        }
        return Enumerable.Empty<SensorData>();
    }

    public void AddMeasureSensor(SensorData sensorData)
    {
        _storage.AddOrUpdate(Guid.Parse(sensorData.SensorId),
            _ => new List<SensorData> {sensorData},
            (_, old) =>
            {
                old.Add(sensorData);
                return old;
            });
    }
}