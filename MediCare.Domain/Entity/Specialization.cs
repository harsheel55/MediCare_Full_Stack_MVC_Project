using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }

        [Required(ErrorMessage = "Specialization name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialization name must be between 2 and 100 characters.")]
        public string SpecializationName { get; set; }

        public int? CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Navigation Property  
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}