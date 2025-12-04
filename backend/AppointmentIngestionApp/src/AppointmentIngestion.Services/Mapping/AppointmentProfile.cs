using AppointmentIngestion.Repositories.Models;
using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Services.Extensions;
using AutoMapper;

namespace AppointmentIngestion.Services.Mapping
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            // DTO2Domain
            CreateMap<AppointmentRequestDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AppointmentTime,
                    opt => opt.MapFrom(src => src.AppointmentTime.NormalizeToUtc()))
                .ForMember(dest => dest.ServiceDurationMinutes,
                    opt => opt.MapFrom(src => src.ServiceDurationMinutes ?? 30));

            // Domain2DTO
            CreateMap<Appointment, AppointmentResponseDto>();
        }
    }
}
