using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class PatientNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientNoteId { get; set; }

        [Required]
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [Url]
        public string NoteUrl { get; set; } // Azure Blob Storage URL

        public int? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property 
        public virtual Appointment Appointment { get; set; }
    }
}