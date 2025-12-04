using AppointmentIngestion.Common;
using AppointmentIngestion.Services.DTOs;
using CSharpFunctionalExtensions;

namespace AppointmentIngestion.Services.Contracts
{
    public interface IAppointmentIngestionService
    {
        Task<Result<int,DomainError>> IngestAsync(AppointmentRequestDto request);
        Task<Result<IReadOnlyList<AppointmentResponseDto>,DomainError>> GetAllAsync();
        Task<Result<AppointmentResponseDto,DomainError>> GetByIdAsync(int id);
    }
}
