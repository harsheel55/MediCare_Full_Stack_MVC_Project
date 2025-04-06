using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Appointment : IValidatableObject
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        public DateOnly AppointmentDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan AppointmentStarts { get; set; }

        public TimeSpan AppointmentEnds { get; set; }

        [Required, StringLength(20, ErrorMessage = "Status can't exceed 20 characters.")]
        public string Status { get; set; }

        [StringLength(255, ErrorMessage = "Description can't exceed 255 characters.")]
        public string AppointmentDescription { get; set; }

        public int? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual PatientNote? PatientNote { get; set; }
        public virtual Invoice? Invoice { get; set; }

        // Custom Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AppointmentEnds <= AppointmentStarts)
            {
                yield return new ValidationResult(
                    "Appointment end time must be after the start time.",
                    new[] { nameof(AppointmentEnds) }
                );
            }

            if (AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                yield return new ValidationResult(
                    "Appointment date cannot be in the past.",
                    new[] { nameof(AppointmentDate) }
                );
            }
        }
    }
}
