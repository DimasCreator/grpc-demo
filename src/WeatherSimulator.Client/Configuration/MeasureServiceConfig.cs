namespace WeatherSimulator.Client.Configuration;

public class MeasureServiceConfig
{
    public string[] SensorIds { get; set; }
    public string GrpcUrl { get; set; }
}