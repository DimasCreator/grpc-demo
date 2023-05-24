using Microsoft.AspNetCore.Mvc;
using WeatherSimulator.Client.Grpc;
using WeatherSimulator.Client.Storages;

namespace WeatherSimulator.Client.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorController : Controller
{
    private readonly IMeasureService _measureService;
    private readonly ISensorStorage _storage;

    public SensorController(IMeasureService measureService, ISensorStorage storage)
    {
        _measureService = measureService;
        _storage = storage;
    }

    [HttpGet("{sensorId}/current-measure")]
    public async Task<IActionResult> GetCurrentMeasureBySensorId([FromRoute] string sensorId, CancellationToken cancellationToken)
    {
        var measure = await _measureService.GetSensorDataByIdAsync(sensorId, cancellationToken);
        return Ok(measure);
    }
    
    [HttpGet("{sensorId:guid}/measures")]
    public IActionResult GetMeasureBySensorId([FromRoute] Guid sensorId, CancellationToken cancellationToken)
    {
        var measure = _storage.GetMeasuresBySensorId(sensorId);
        return Ok(measure);
    }
}

