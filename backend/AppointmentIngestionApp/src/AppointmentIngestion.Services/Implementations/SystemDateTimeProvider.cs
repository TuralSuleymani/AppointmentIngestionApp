using AppointmentIngestion.Services.Contracts;

namespace AppointmentIngestion.Services.Implementations
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
