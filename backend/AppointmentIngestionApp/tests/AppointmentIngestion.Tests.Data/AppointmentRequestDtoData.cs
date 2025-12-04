namespace AppointmentIngestion.Tests.Data
{
    public static class AppointmentRequestDtoData
    {
        public const string ValidCustomer = "Rowan Miller";
        public static DateTime ValidAppointmentTime = DateTime.UtcNow.AddMinutes(10);
        public static int ValidServiceDurationMinutes = 10;
    }
}
