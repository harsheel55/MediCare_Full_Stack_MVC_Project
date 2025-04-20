namespace MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs
{
    public class GetAppointmentDTO
    {
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;  }
        public string DoctorName { get; set; }
        public string Email { get; set; }
        public string AadharNo { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan AppointmentStarts { get; set; }
        public TimeSpan AppointmentEnds { get; set; }
        public string Status { get; set; }
        public string AppointmentDescription { get; set; }
    }
}