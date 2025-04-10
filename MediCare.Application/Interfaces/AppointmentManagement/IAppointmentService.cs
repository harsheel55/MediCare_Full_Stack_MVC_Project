using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement
{
    public interface IAppointmentService
    {
        Task<ICollection<GetAppointmentDTO>> GetAllAppointmentAsync();
        Task BookAppointmentAsync(int id, AppointmentDTO doctor);
        Task DeleteAppointmentByIdAsync(int id);
        Task SendReminderEmailAsync(int id);
    }
}