using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class PatientAdmission : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdmissionId { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey("Bed")]
        public int BedId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateOnly AdmissionDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateOnly? DischargeDate { get; set; }

        public bool IsDischarged { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Bed Bed { get; set; }
        public virtual Patient Patient { get; set; }

        // Custom Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DischargeDate.HasValue && DischargeDate <= AdmissionDate)
            {
                yield return new ValidationResult(
                    "Discharge date must be after admission date.",
                    new[] { nameof(DischargeDate) }
                );
            }
        }
    }
}