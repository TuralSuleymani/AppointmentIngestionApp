using AppointmentIngestion.Repositories.Models;

namespace AppointmentIngestion.Repositories.Contracts
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(int id);
        Task<IReadOnlyList<Appointment>> GetAllAsync();
    }
}
