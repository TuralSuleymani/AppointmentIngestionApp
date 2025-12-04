namespace AppointmentIngestion.Services.DTOs
{
    public class AppointmentRequestDto
    {
        public string ClientName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int? ServiceDurationMinutes { get; set; }
    }
}
