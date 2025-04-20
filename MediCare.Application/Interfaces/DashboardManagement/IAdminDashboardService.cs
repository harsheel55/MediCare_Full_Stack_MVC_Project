using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IAdminDashboardService
    {
        // ----- Basic Stats -----
        Task<int> GetTotalDoctorsAsync();
        Task<int> GetTotalPatientsAsync();
        Task<int> GetTodayAppointmentsCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalAppointmentsAsync();
        Task<int> GetTotalReceptionistsAsync();
        Task<int> GetTotalLabTestsAsync();
        Task<int> GetAvailableBedsCountAsync();

        // ----- Lists -----
        Task<List<dynamic>> GetRecentAppointmentsAsync(); // You can replace 'dynamic' with a DTO class like AppointmentSummaryDTO
        Task<List<dynamic>> GetTopPerformingDoctorsAsync(); // Replace with a DTO like DoctorPerformanceDTO

        // ----- Charts -----
        Task<List<int>> GetWeeklyAppointmentsAsync(); // List of counts Mon–Sun
        Task<Dictionary<string, decimal>> GetRevenueBreakdownAsync(); // Revenue by category
    }
}
