namespace AppointmentIngestion.Services.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime NormalizeToUtc(this DateTime datetime)
        {
            if (datetime.Kind == DateTimeKind.Utc)
                return datetime;

            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
        }
    }
}
