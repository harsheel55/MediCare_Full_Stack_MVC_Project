using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement
{
    public interface IAppointmentRepository
    {
        Task<ICollection<GetAppointmentDTO>> GetAllAppointmentQuery();
        Task BookAppointmentQuery(int id, AppointmentDTO doctor);
        Task DeleteAppointmentByIdQuery(int id);
        Task SendReminderEmailQuery(int id);
    }
}