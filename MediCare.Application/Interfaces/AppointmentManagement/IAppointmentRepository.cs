using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement
{
    public interface IAppointmentRepository
    {
        Task<ICollection<GetAppointmentDTO>> GetAllAppointmentQuery();
        Task BookAppointmentQuery(int id, AppointmentDTO doctor);
        Task DeleteAppointmentByIdQuery(int id);
        Task SendReminderEmailQuery(int id);
        Task<GetAppointmentDTO> GetAppointmentByIdSendMailQuery(int id);
        Task<Appointment> GetAppointmentByPatientIdQuery(int id, DateOnly date);
        Task UpdateAppointmentQuery(int id, AppointmentDTO appointment, int updatedBy, DateOnly date);
        Task<ICollection<GetAppointmentDTO>> GetAppointmentByDoctorIdQuery(int id);
    }
}