using AppointmentIngestion.Services.DTOs;
using FluentValidation;

namespace AppointmentIngestion.Services.Validation
{
    public class AppointmentRequestValidator : AbstractValidator<AppointmentRequestDto>
    {
        public AppointmentRequestValidator()
        {
            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("ClientName is required.");

            RuleFor(x => x.AppointmentTime)
                .NotNull().WithMessage("AppointmentTime is required.")
                .Must(BeAtLeast5MinutesInFuture)
                .WithMessage("Appointment time must be at least 5 minutes in the future.")
                .Must(StartOnHourOrHalfHour)
                .WithMessage("Appointment must start on the hour or half-hour.");

            RuleFor(x => x.ServiceDurationMinutes)
                .GreaterThan(0)
                .When(x => x.ServiceDurationMinutes.HasValue)
                .WithMessage("ServiceDurationMinutes must be positive.");
        }

        private bool BeAtLeast5MinutesInFuture(DateTime? datetime)
            => datetime != null && datetime.Value.ToUniversalTime() >= DateTime.UtcNow.AddMinutes(5);

        private bool StartOnHourOrHalfHour(DateTime? datetime)
            => datetime != null && (datetime.Value.Minute == 0 || datetime.Value.Minute == 30);
    }
}
