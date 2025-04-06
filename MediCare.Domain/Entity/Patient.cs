using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required, StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required, StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10)]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "AadharNo is required.")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar number must be exactly 12 digits.")]
        [RegularExpression("^[0-9]{12}$", ErrorMessage = "Aadhar number must contain only digits.")]
        public string AadharNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid mobile number.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public bool Active { get; set; } = true;

        public int? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual PatientAdmission PatientAdmission { get; set; }
        public virtual ICollection<PatientTest> PatientTests { get; set; }
    }
}