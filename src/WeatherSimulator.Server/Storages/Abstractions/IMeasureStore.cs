using System;
using System.Threading.Tasks;
using WeatherSimulator.Server.Models;

namespace WeatherSimulator.Server.Storages.Abstractions;

public interface IMeasureStore
{
    public void UpdateMeasure(SensorMeasure measure);

    public Task<SensorMeasure?> GetSensorMeasure(Guid sensorId);
}