namespace MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs
{
    public class AppointmentDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan AppointmentStarts { get; set; }
        public TimeSpan AppointmentEnds { get; set; }
        public string Status { get; set; }
        public string AppointmentDescription { get; set; }
        public int? CreatedBy { get; set; }
    }
}