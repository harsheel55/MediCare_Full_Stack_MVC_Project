using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Room number must be between 1 and 50 characters.")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public string RoomType { get; set; } // e.g., ICU, General, Private

        [Display(Name = "Occupied")]
        public bool IsOccupied { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100, ErrorMessage = "Creator name can't exceed 100 characters.")]
        public string CreatedBy { get; set; }

        // Navigation Property 
        public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();
    }
}