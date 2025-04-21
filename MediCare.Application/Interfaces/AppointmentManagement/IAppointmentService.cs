using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement
{
    public interface IAppointmentService
    {
        Task<ICollection<GetAppointmentDTO>> GetAllAppointmentAsync();
        Task BookAppointmentAsync(int id, AppointmentDTO doctor);
        Task DeleteAppointmentByIdAsync(int id);
        Task SendReminderEmailAsync(int id);
        Task<Appointment> GetAppointmentByIdAsync(int id, DateOnly date);
        Task UpdateAppointmentAsync(int id, AppointmentDTO appointment, int updatedBy, DateOnly date);
        Task<ICollection<GetAppointmentDTO>> GetAppointmentByDoctorIdAsync(int id);
    }
}