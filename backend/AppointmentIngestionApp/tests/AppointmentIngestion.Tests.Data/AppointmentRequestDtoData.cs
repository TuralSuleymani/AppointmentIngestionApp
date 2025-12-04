using AppointmentIngestion.Services.Contracts;

namespace AppointmentIngestion.Tests.Data
{
    public static class AppointmentRequestDtoData
    {
        public const string ValidCustomer = "Rowan Miller";
        public static DateTime ValidAppointmentTime(IDateTimeProvider datetimeProvider) => datetimeProvider.UtcNow.AddMinutes(70)
                                                                                                        .AddSeconds(-datetimeProvider.UtcNow.AddMinutes(10).Second)
                                                                                                            .AddMinutes(-(datetimeProvider.UtcNow.AddMinutes(10).Minute % 30));
        public static int ValidServiceDurationMinutes = 10;
    }
}
