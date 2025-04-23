using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IDoctorDashboardRepository
    {
        Task<int> GetTotalAppointmentQuery(int doctorId);
        Task<int> GetCompletedAppointmentQuery(int doctorId);
        Task<int> GetPendingAppointmentQuery(int doctorId);
        Task<decimal> GetTotalRevenueQuery(int doctorId);
        Task<ICollection<DocDashboardDTO>> GetUpcomingAppointmentQuery(int doctorId);
    }
}
