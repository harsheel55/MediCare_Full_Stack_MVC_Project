namespace MediCare_MVC_Project.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DoctorName { get; set; }
        public string Email { get; set; }
        public string AadharNo { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string AppointmentStarts { get; set; }
        public string AppointmentEnds { get; set; }
        public string Status { get; set; }
        public string AppointmentDescription { get; set; }
    }
}