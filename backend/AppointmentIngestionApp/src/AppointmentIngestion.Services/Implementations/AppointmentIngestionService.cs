using AppointmentIngestion.Common;
using AppointmentIngestion.Repositories.Contracts;
using AppointmentIngestion.Repositories.Models;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace AppointmentIngestion.Services.Implementations;

public class AppointmentIngestionService : IAppointmentIngestionService
{
    private readonly IAppointmentRepository _repository;
    private readonly IValidator<AppointmentRequestDto> _validator;
    private readonly IMapper _mapper;

    public AppointmentIngestionService(
        IAppointmentRepository repository,
        IValidator<AppointmentRequestDto> validator,
        IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<Result<int, IDomainError>> IngestAsync(AppointmentRequestDto request)
    {
        var validation = await _validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            return Result.Failure<int, IDomainError>(
                DomainError.Validation("Validation Failed.", errors)
            );
        }

        var appointment = _mapper.Map<Appointment>(request);

        var saved = await _repository.AddAsync(appointment);
        return Result.Success<int, IDomainError>(saved.Id);
    }

    public async Task<Result<IReadOnlyList<AppointmentResponseDto>, IDomainError>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();

        var dtos = _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(items);

        return Result.Success<IReadOnlyList<AppointmentResponseDto>, IDomainError>(dtos);
    }

    public async Task<Result<AppointmentResponseDto, IDomainError>> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return Result.Failure<AppointmentResponseDto, IDomainError>(
                DomainError.NotFound($"Appointment with ID {id} was not found.")
            );
        }

        return _mapper.Map<AppointmentResponseDto>(entity);
    }
}
