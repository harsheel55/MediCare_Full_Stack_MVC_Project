using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Bed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BedId { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [Required]
        public int BedNumber { get; set; }

        public bool IsOccupied { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Room Room { get; set; }
        public virtual PatientAdmission PatientAdmission { get; set; }
    }
}