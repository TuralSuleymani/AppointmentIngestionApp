using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Tests.Data;

namespace AppointmentIngestion.Tests.Unit.Factories
{
    public static class AppointmentRequestDtoFactory
    {
        public static AppointmentRequestDto Create(IDateTimeProvider datetimeProvider, string? clientName = null,
            DateTime? appointmentTime = null, int? serviceDurationMinutes = null)
            => new()
            {
                ClientName = clientName ?? AppointmentRequestDtoData.ValidCustomer,
                AppointmentTime = appointmentTime ?? AppointmentRequestDtoData.ValidAppointmentTime(datetimeProvider),
                ServiceDurationMinutes = serviceDurationMinutes ?? AppointmentRequestDtoData.ValidServiceDurationMinutes
            };
    }
}
