using AppointmentIngestion.Repositories.Models;

namespace AppointmentIngestion.Tests.Data
{
    public static class AppointmentData
    {
        public static int ValidAppointmentId = 10;
        public static List<Appointment> ValidAppointments
            = new List<Appointment>
            {
                new Appointment { Id = 1 },
                new Appointment { Id = 2 }
            };
    }
}
