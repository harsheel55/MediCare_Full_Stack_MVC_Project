using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IReceptionistDashboardRepository
    {
        Task<int> GetTotalAppointmentCountQuery();
        Task<int> GetTotalPatientCountQuery();
        Task<int> GetTodaysAppointmentCountQuery();
        Task<decimal> GetTotalRevenueQuery();
        Task<ICollection<ReceptionistDashDTO>> GetRecentAppointmentQuery();
        Task<ICollection<ReceptionistUpcomingDTO>> GetUpcomingAppointmentQuery();
    }
}
