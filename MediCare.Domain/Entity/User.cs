using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfJoining { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfRelieving { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Phone(ErrorMessage = "Invalid emergency contact number.")]
        [StringLength(10)]
        public string EmergencyNo { get; set; }

        [Required(ErrorMessage = "Password hash is required.")]
        [StringLength(255, ErrorMessage = "Password hash must not exceed 255 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^])[A-Za-z\d@$!%*?&#^]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; } // Store hash, not plain-text

        [Required]
        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "Role is required.")]
        [Range(1, 3)]
        public int RoleId { get; set; }

        public int? CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Role Role { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Receptionist Receptionist { get; set; }

        // Custom Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfJoining.HasValue && DateOfRelieving.HasValue && DateOfJoining > DateOfRelieving)
            {
                yield return new ValidationResult(
                    "Date of relieving must be after date of joining.",
                    new[] { nameof(DateOfRelieving) }
                );
            }
        }
    }
}