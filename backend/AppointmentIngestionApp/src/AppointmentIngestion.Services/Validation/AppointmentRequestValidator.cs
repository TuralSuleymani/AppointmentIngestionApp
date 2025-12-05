using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using FluentValidation;

namespace AppointmentIngestion.Services.Validation
{
    public class AppointmentRequestValidator : AbstractValidator<AppointmentRequestDto>
    {
        private readonly IDateTimeProvider _datetimeProvider;
        public AppointmentRequestValidator(IDateTimeProvider datetimeProvider)
        {
            _datetimeProvider = datetimeProvider;
            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage(ValidationErrors.ClientNameRequired);

            RuleFor(x => x.AppointmentDate)
                .NotNull().WithMessage(ValidationErrors.AppointmentDateRequired)
                .Must(BeAtLeast5MinutesInFuture)
                .WithMessage(ValidationErrors.AppointmentDateFuture)
                .Must(StartOnHourOrHalfHour)
                .WithMessage(ValidationErrors.AppointmentDateSlot);

            RuleFor(x => x.ServiceDurationMinutes)
                .GreaterThan(0)
                .When(x => x.ServiceDurationMinutes.HasValue)
                .WithMessage(ValidationErrors.ServiceDurationPositive);
        }

        private bool BeAtLeast5MinutesInFuture(DateTime datetime)
            => datetime.ToUniversalTime() >= _datetimeProvider.UtcNow.AddMinutes(5);

        private bool StartOnHourOrHalfHour(DateTime datetime)
            => datetime.Minute == 0 || datetime.Minute == 30;
    }
}
