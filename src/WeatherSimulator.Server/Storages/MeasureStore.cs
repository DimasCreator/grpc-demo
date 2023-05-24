using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WeatherSimulator.Server.Models;
using WeatherSimulator.Server.Storages.Abstractions;

namespace WeatherSimulator.Server.Storages;

public class MeasureStore : IMeasureStore
{
    private readonly ConcurrentDictionary<Guid, SensorMeasure> _measures = new();

    public void UpdateMeasure(SensorMeasure measure)
    {
        _measures[measure.SensorId] = measure;
    }

    public Task<SensorMeasure?> GetSensorMeasure(Guid sensorId)
    {
        _measures.TryGetValue(sensorId, out var measure);
        return Task.FromResult(measure);
    }
}