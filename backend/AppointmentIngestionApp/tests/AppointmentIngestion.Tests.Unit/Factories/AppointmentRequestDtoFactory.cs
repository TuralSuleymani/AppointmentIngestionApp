using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Tests.Data;

namespace AppointmentIngestion.Tests.Unit.Factories
{
    public static class AppointmentRequestDtoFactory
    {
        public static AppointmentRequestDto Create(string? clientName = null,
            DateTime? appointmentTime = null, int? serviceDurationMinutes = null)
            => new()
            {
                  ClientName = clientName ?? AppointmentRequestDtoData.ValidCustomer
                , AppointmentTime = appointmentTime ?? AppointmentRequestDtoData.ValidAppointmentTime
                , ServiceDurationMinutes = serviceDurationMinutes ?? AppointmentRequestDtoData.ValidServiceDurationMinutes
            };
    }
}
