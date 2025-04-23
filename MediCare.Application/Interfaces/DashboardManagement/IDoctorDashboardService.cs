using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement
{
    public interface IDoctorDashboardService
    {
        Task<int> GetTotalAppointmentAsync(int doctorId);
        Task<int> GetCompletedAppointmentAsync(int doctorId);
        Task<int> GetPendingAppointmentAsync(int doctorId);
        Task<decimal> GetTotalRevenueAsync(int doctorId);
        Task<ICollection<DocDashboardDTO>> GetUpcomingAppointmentAsync(int doctorId);
    }
}
