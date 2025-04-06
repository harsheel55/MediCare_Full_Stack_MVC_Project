using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DateOnly, DateTime>()
               .ConvertUsing(dateOnly => dateOnly.ToDateTime(new TimeOnly(0, 0))); // Convert DateOnly to DateTime at midnight

            CreateMap<DateTime, DateOnly>()
                .ConvertUsing(dateTime => DateOnly.FromDateTime(dateTime)); // Convert DateTime to DateOnly

            // Example mapping for UserDTO to UserViewModel
            CreateMap<UserDTO, UserViewModel>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Doctor mappings
            CreateMap<UserDoctorDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            CreateMap<UserDoctorDTO, Doctor>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // Receptionist Mapping
            CreateMap<UserReceptionistDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            CreateMap<UserReceptionistDTO, Receptionist>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}