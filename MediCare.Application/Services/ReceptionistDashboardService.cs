using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class ReceptionistDashboardService : IReceptionistDashboardService
    {
        private readonly IReceptionistDashboardRepository _receptionistDashboardRepository;

        public ReceptionistDashboardService(IReceptionistDashboardRepository receptionistDashboardRepository)
        {
            _receptionistDashboardRepository = receptionistDashboardRepository;
        }

        public async Task<ICollection<ReceptionistDashDTO>> GetRecentAppointmentAsync()
        {
            return await _receptionistDashboardRepository.GetRecentAppointmentQuery();
        }

        public async Task<int> GetTodaysAppointmentCountAsync()
        {
            return await _receptionistDashboardRepository.GetTodaysAppointmentCountQuery();
        }

        public async Task<int> GetTotalAppointmentCountAsync()
        {
            return await _receptionistDashboardRepository.GetTotalAppointmentCountQuery();
        }

        public async Task<int> GetTotalPatientCountAsync()
        {
            return await _receptionistDashboardRepository.GetTotalPatientCountQuery();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _receptionistDashboardRepository.GetTotalRevenueQuery();
        }

        public async Task<ICollection<ReceptionistUpcomingDTO>> GetUpcomingAppointmentAsync()
        {
            return await _receptionistDashboardRepository.GetUpcomingAppointmentQuery();
        }
    }
}
