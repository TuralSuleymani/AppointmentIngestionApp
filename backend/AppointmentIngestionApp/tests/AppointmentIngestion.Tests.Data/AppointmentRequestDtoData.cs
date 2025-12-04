using AppointmentIngestion.Repositories.Models;
using AppointmentIngestion.Services.DTOs;

namespace AppointmentIngestion.Tests.Data
{
    public static class AppointmentRequestDtoData
    {
        public const string ValidCustomer = "Rowan Miller";
        public static DateTime ValidAppointmentTime = DateTime.UtcNow.AddMinutes(10);
        public static int ValidServiceDurationMinutes = 10;
    }

    public static class AppointmentResponseDtoData
    {
        public static List<AppointmentResponseDto> ValidAppointments
         = new List<AppointmentResponseDto>
         {
                new AppointmentResponseDto { Id = 1 },
                new AppointmentResponseDto { Id = 2 }
         };
    }
}
