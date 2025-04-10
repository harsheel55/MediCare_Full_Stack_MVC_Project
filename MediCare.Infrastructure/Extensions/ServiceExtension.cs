using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using MediCare_MVC_Project.MediCare.Application.Services;
using MediCare_MVC_Project.MediCare.Common.Helpers;
using MediCare_MVC_Project.MediCare.Infrastructure.Repository;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ICheckUpRepository, CheckUpRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthHelper>(); 
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<JWTTokenHelper>();
            //services.AddScoped<IDownloadHelper, NoteDownloadHelper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICheckUpService, CheckUpService>();
            services.AddScoped<IReceptionistService, ReceptionistService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            //services.AddScoped<IPatientNotesService, PatientNotesService>();
            //services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}