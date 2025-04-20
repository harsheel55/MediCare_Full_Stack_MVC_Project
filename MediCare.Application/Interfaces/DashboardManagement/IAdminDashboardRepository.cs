namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IAdminDashboardRepository
    {
        // ----- Basic Stats -----
        Task<int> GetTotalDoctorsQuery();
        Task<int> GetTotalPatientsQuery();
        Task<int> GetTodayAppointmentsCountQuery();
        Task<decimal> GetTotalRevenueQuery();
        Task<int> GetTotalAppointmentsQuery();
        Task<int> GetTotalReceptionistsQuery();
        Task<int> GetTotalLabTestsQuery();
        Task<int> GetAvailableBedsCountQuery();

        // ----- Lists -----
        Task<List<dynamic>> GetRecentAppointmentsQuery(); // You can replace 'dynamic' with a DTO class like AppointmentSummaryDTO
        Task<List<dynamic>> GetTopPerformingDoctorsQuery(); // Replace with a DTO like DoctorPerformanceDTO

        // ----- Charts -----
        Task<List<int>> GetWeeklyAppointmentsQuery(); // List of counts Mon–Sun
        Task<Dictionary<string, decimal>> GetRevenueBreakdownQuery(); // Revenue by category
    }
}
