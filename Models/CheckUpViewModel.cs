namespace MediCare_MVC_Project.Models
{
    public class CheckUpViewModel
    {
        public int PatientNoteId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AppointmentDescription { get; set; }
        public string NoteText { get; set; }
    }
}