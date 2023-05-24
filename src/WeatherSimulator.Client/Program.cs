using WeatherSimulator.Client;

public class Program
{
    public static Task Main(string[] args) =>
        CreateHostBuilder(args)
            .Build()
            .RunAsync();


    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}