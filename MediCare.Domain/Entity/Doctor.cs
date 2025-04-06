using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Qualification must not exceed 100 characters.")]
        public string Qualification { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "License number must not exceed 50 characters.")]
        public string LicenseNumber { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
