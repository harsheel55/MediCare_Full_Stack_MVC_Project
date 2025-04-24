using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IReceptionistDashboardService
    {
        Task<int> GetTotalAppointmentCountAsync();
        Task<int> GetTotalPatientCountAsync();
        Task<int> GetTodaysAppointmentCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<ICollection<ReceptionistDashDTO>> GetRecentAppointmentAsync();
        Task<ICollection<ReceptionistUpcomingDTO>> GetUpcomingAppointmentAsync();
    }
}