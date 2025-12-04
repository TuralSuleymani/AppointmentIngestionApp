using AppointmentIngestion.Services.DTOs;

namespace AppointmentIngestion.Tests.Data
{
    public static class AppointmentResponseDtoData
    {
        public static List<AppointmentResponseDto> ValidAppointments
         = new List<AppointmentResponseDto>
         {
                new AppointmentResponseDto { Id = 1 },
                new AppointmentResponseDto { Id = 2 }
         };

        public static int AppointmentId = 10;
    }
}
