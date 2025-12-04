using AppointmentIngestion.Repositories.Contracts;
using AppointmentIngestion.Repositories.Models;

namespace AppointmentIngestion.Repositories.Implementations
{
    public class InMemoryAppointmentRepository : IAppointmentRepository
    {
        private static readonly List<Appointment> _appointments = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();

        public Task<Appointment> AddAsync(Appointment appointment)
        {
            lock (_lock)
            {
                appointment.Id = _nextId++;
                _appointments.Add(appointment);
            }

            return Task.FromResult(appointment);
        }

        public Task<IReadOnlyList<Appointment>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult((IReadOnlyList<Appointment>)[.. _appointments]);
            }
        }

        public Task<Appointment?> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                var appt = _appointments.FirstOrDefault(a => a.Id == id);
                return Task.FromResult(appt);
            }
        }
    }
}
