using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Emergency contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Emergency number must be 10 digits.")]
        public string EmergencyNo { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(Admin|Doctor|Patient|Receptionist)$", ErrorMessage = "Role must be 'Admin', 'Doctor', 'Patient', or 'Receptionist'.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public bool Status { get; set; }
    }

}
