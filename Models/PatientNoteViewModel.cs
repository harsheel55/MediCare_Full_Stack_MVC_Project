using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.Models
{
    public class PatientNoteViewModel
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public DateOnly AppointmentDate { get; set; }

        [Required(ErrorMessage = "Note text is required")]
        [Display(Name = "Note")]
        [DataType(DataType.MultilineText)]
        public string NoteText { get; set; }
    }
}