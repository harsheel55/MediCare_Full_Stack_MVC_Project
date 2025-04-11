namespace MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs
{
    public class PdfNoteDTO
    {
        public string PatientName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AadharNo { get; set; }
        public string MobileNo { get; set; }
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
        public string NoteText { get; set; }
    }
}