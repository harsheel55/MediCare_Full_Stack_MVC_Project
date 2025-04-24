namespace MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard
{
    public class ReceptionistDashDTO
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string Status { get; set; }
    }
}