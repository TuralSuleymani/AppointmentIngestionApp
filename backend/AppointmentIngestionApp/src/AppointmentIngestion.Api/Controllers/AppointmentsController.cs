using AppointmentIngestion.Api.Controllers;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentIngestion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : BaseController
{
    private readonly IAppointmentIngestionService _service;

    public AppointmentsController(IAppointmentIngestionService service
        , ILogger<AppointmentsController> logger)
        : base(logger)
    {
        _service = service;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest([FromBody] AppointmentRequestDto request)
    {
        var result = await _service.IngestAsync(request);

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new
        {
            id = result.Value,
            message = "Appointment created successfully."
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(result.Value);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(result.Value);
    }

}
