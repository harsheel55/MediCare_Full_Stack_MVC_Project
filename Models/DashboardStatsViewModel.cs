namespace MediCare_MVC_Project.Models
{
    public class DashboardStatsViewModel
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalReceptionists { get; set; }
        public int TotalLabTests { get; set; }
        public int AppointmentsToday { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalAppointments { get; set; }
        public int AvailableBeds { get; set; }
    }
}