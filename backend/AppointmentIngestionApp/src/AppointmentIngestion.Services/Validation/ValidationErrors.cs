namespace AppointmentIngestion.Services.Validation
{
    public static class ValidationErrors
    {
        public const string ClientNameRequired = "ClientName is required.";
        public const string AppointmentDateRequired = "AppointmentDate is required.";
        public const string AppointmentDateFuture = "Appointment time must be at least 5 minutes in the future.";
        public const string AppointmentDateSlot = "Appointment must start on the hour or half-hour.";
        public const string ServiceDurationPositive = "ServiceDurationMinutes must be positive.";
        public static string AppointmentNotFound(int id)
        => $"Appointment with ID {id} was not found.";
    }

}
