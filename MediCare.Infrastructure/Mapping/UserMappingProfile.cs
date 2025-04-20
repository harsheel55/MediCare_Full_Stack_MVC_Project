using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<GetDoctorDTO, DoctorViewModel>();
            CreateMap<GetPatientDTO, PatientViewModel>();
            CreateMap<GetAppointmentDTO, AppointmentViewModel>();
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<GetCheckUpDTO, CheckUpViewModel>();
            CreateMap<GetPaymentDTO, PaymentInvoiceViewModel>();
            CreateMap<LabTest, LabTestViewModel>();
            CreateMap<GetPatientTestDTO, PatientTestViewModel>();
            CreateMap<CheckUpDTO, PatientNoteViewModel>();
            CreateMap<GetRoomDTO, RoomViewModel>();
            CreateMap<GetAdmissionDTO, AdmissionViewModel>();
            CreateMap<GetBedDTO,  BedViewModel>();

            CreateMap<DateOnly, DateTime>()
               .ConvertUsing(dateOnly => dateOnly.ToDateTime(new TimeOnly(0, 0))); 

            CreateMap<DateTime, DateOnly>()
                .ConvertUsing(dateTime => DateOnly.FromDateTime(dateTime));

            // Map from UserDTO → UserRegisterDTO (for Edit)
            CreateMap<UserDTO, UserRegisterDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // Map UserId → Id
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Status)) // Map Status → Active
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Skip password (security)
                .ForMember(dest => dest.RoleId, opt => opt.Ignore()); // RoleId is not in UserDTO

            // Map from UserRegisterDTO → UserDTO (for Create/Update)
            CreateMap<UserRegisterDTO, UserDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // Map Id → UserId
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Active)) // Map Active → Status
                .ForMember(dest => dest.Role, opt => opt.Ignore());


            // Example mapping for UserDTO to UserViewModel
            CreateMap<UserDTO, UserViewModel>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Doctor mappings
            CreateMap<UserDoctorDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Avoid mapping raw password
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2)); // Doctor role

            CreateMap<UserDoctorDTO, Doctor>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // Link to existing user
                .ForMember(dest => dest.User, opt => opt.Ignore()) // Don't map navigation
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());


            // Receptionist Mapping
            CreateMap<GetReceptionistDTO, ReceptionistViewModel>();

            CreateMap<UserReceptionistDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            CreateMap<UserReceptionistDTO, Receptionist>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}