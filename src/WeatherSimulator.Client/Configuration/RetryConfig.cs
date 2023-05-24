namespace WeatherSimulator.Client.Configuration;

public class RetryConfig
{
    public int MinRetryDelaySeconds { get; set; }
    public int MaxRetryDelaySeconds { get; set; }
}