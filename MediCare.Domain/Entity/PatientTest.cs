using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class PatientTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientTestId { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey("LabTest")]
        public int TestId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateOnly TestDate { get; set; }

        [StringLength(500, ErrorMessage = "Result cannot exceed 500 characters.")]
        public string? Result { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual LabTest LabTest { get; set; }
    }
}