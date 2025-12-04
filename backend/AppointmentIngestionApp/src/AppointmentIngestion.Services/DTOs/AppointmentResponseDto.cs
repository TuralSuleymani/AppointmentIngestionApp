namespace AppointmentIngestion.Services.DTOs
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = default!;
        public DateTime AppointmentTime { get; set; }
        public int ServiceDurationMinutes { get; set; }
    }
}
