using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Receptionist
    {
        [Key]
        public int ReceptionistId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Specification is required.")]
        [StringLength(100)]
        public string Qualification { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}