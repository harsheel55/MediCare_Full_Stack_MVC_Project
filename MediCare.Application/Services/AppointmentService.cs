using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task BookAppointmentAsync(int id, AppointmentDTO appointment)
        {
            await _appointmentRepository.BookAppointmentQuery(id, appointment);
        }

        public async Task DeleteAppointmentByIdAsync(int id)
        {
            await _appointmentRepository.DeleteAppointmentByIdQuery(id);
        }

        public async Task<ICollection<GetAppointmentDTO>> GetAllAppointmentAsync()
        {
            var appointmentList = await _appointmentRepository.GetAllAppointmentQuery();
            return appointmentList;
        }

        public Task SendReminderEmailAsync(int id)
        {
            return _appointmentRepository.SendReminderEmailQuery(id);
        }
    }
}