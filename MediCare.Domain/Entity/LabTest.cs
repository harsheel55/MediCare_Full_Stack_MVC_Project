using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class LabTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Test name must be between 3 and 100 characters.")]
        public string TestName { get; set; }

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0, 99999.99, ErrorMessage = "Cost must be a positive value less than 99999.99.")]
        public decimal Cost { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual ICollection<PatientTest> PatientTests { get; set; }
    }
}