using AppointmentIngestion.Common;
using AppointmentIngestion.Repositories.Contracts;
using AppointmentIngestion.Repositories.Models;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Serilog.Core;
namespace AppointmentIngestion.Services.Implementations;

public class AppointmentIngestionService : IAppointmentIngestionService
{
    private readonly IAppointmentRepository _repository;
    private readonly IValidator<AppointmentRequestDto> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentIngestionService> _logger;
    public AppointmentIngestionService(
        IAppointmentRepository repository,
        IValidator<AppointmentRequestDto> validator,
        IMapper mapper,
        ILogger<AppointmentIngestionService> logger)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<int, IDomainError>> IngestAsync(AppointmentRequestDto request)
    {
        _logger.LogInformation("Ingesting new appointment for {ClientName} at {Time}",
        request.ClientName,
        request.AppointmentTime);

        var validation = await _validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            _logger.LogWarning("Validation failed for appointment request {@Request}. Errors: {@Errors}",
            request, errors);

            return Result.Failure<int, IDomainError>(
                DomainError.Validation(errors: errors)
            );
        }

        var appointment = _mapper.Map<Appointment>(request);

        _logger.LogDebug("Mapped Appointment entity: {@Appointment}", appointment);

        var saved = await _repository.AddAsync(appointment);

        _logger.LogInformation("Appointment created successfully with ID {Id}", saved.Id);

        return Result.Success<int, IDomainError>(saved.Id);
    }

    public async Task<Result<IReadOnlyList<AppointmentResponseDto>, IDomainError>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all appointments from repository");

        var items = await _repository.GetAllAsync();

        _logger.LogInformation(
        "Retrieved {Count} appointments from repository",
        items.Count
        );

        _logger.LogDebug("Repository returned appointments: {@Appointments}", items);

        var dtos = _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(items);

        _logger.LogDebug("Mapped to response DTOs: {@Dtos}", dtos);

        return Result.Success<IReadOnlyList<AppointmentResponseDto>, IDomainError>(dtos);
    }

    public async Task<Result<AppointmentResponseDto, IDomainError>> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching appointment ID {Id}", id);

        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            _logger.LogWarning("Appointment with ID {Id} was not found", id);

            return Result.Failure<AppointmentResponseDto, IDomainError>(
                DomainError.NotFound($"Appointment with ID {id} was not found.")
            );
        }

        _logger.LogInformation("Appointment fetched successfully: {@Appointment}", entity);

        return _mapper.Map<AppointmentResponseDto>(entity);
    }
}
