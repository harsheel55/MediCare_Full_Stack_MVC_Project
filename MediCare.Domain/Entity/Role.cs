using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50)]
        public string RoleName { get; set; }

        // Navigation Property
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}