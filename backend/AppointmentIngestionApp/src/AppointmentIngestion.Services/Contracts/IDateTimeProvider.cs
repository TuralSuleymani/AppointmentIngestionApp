namespace AppointmentIngestion.Services.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
