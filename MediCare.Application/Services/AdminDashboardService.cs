using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;

        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository)
        {
            _adminDashboardRepository = adminDashboardRepository;
        }

        public async Task<int> GetAvailableBedsCountAsync()
        {
            return await _adminDashboardRepository.GetAvailableBedsCountQuery();
        }

        public async Task<List<dynamic>> GetRecentAppointmentsAsync()
        {
            return await _adminDashboardRepository.GetRecentAppointmentsQuery();
        }

        public async Task<Dictionary<string, decimal>> GetRevenueBreakdownAsync()
        {
            return await _adminDashboardRepository.GetRevenueBreakdownQuery();
        }

        public async Task<int> GetTodayAppointmentsCountAsync()
        {
            return await _adminDashboardRepository.GetTodayAppointmentsCountQuery();
        }

        public async Task<List<dynamic>> GetTopPerformingDoctorsAsync()
        {
            return await _adminDashboardRepository.GetTopPerformingDoctorsQuery();
        }

        public async Task<int> GetTotalAppointmentsAsync()
        {
            return await _adminDashboardRepository.GetTotalAppointmentsQuery();
        }

        public async Task<int> GetTotalDoctorsAsync()
        {
            return await _adminDashboardRepository.GetTotalDoctorsQuery();
        }

        public async Task<int> GetTotalLabTestsAsync()
        {
            return await _adminDashboardRepository.GetTotalLabTestsQuery();
        }

        public async Task<int> GetTotalPatientsAsync()
        {
            return await _adminDashboardRepository.GetTotalPatientsQuery();
        }

        public async Task<int> GetTotalReceptionistsAsync()
        {
            return await _adminDashboardRepository.GetTotalReceptionistsQuery();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _adminDashboardRepository.GetTotalRevenueQuery();
        }

        public async Task<List<int>> GetWeeklyAppointmentsAsync()
        {
            return await _adminDashboardRepository.GetWeeklyAppointmentsQuery();
        }
    }
}