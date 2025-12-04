using AppointmentIngestion.Common;
using AppointmentIngestion.Services.DTOs;
using CSharpFunctionalExtensions;

namespace AppointmentIngestion.Services.Contracts
{
    public interface IAppointmentIngestionService
    {
        Task<Result<int, IDomainError>> IngestAsync(AppointmentRequestDto request);
        Task<Result<IReadOnlyList<AppointmentResponseDto>, IDomainError>> GetAllAsync();
        Task<Result<AppointmentResponseDto, IDomainError>> GetByIdAsync(int id);
    }
}
