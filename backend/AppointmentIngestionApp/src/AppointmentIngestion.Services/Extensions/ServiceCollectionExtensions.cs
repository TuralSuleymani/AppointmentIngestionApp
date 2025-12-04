using AppointmentIngestion.Repositories.Contracts;
using AppointmentIngestion.Repositories.Implementations;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Services.Implementations;
using AppointmentIngestion.Services.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentIngestion.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesLayer(this IServiceCollection services)
        {
            return services
                 .AddSingleton<IAppointmentRepository, InMemoryAppointmentRepository>()
                 .AddScoped<IAppointmentIngestionService, AppointmentIngestionService>()
                 .AddScoped<IValidator<AppointmentRequestDto>, AppointmentRequestValidator>();
        }
    }
}
