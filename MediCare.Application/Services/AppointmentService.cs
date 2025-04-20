using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;

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

        public Task<Appointment> GetAppointmentByIdAsync(int id, DateOnly date)
        {
            return _appointmentRepository.GetAppointmentByIdQuery(id, date);
        }

        public Task SendReminderEmailAsync(int id)
        {
            return _appointmentRepository.SendReminderEmailQuery(id);
        }

        public async Task UpdateAppointmentAsync(int id, AppointmentDTO appointment, int updatedBy, DateOnly date)
        {
            await _appointmentRepository.UpdateAppointmentQuery(id, appointment, updatedBy, date);
        }
    }
}