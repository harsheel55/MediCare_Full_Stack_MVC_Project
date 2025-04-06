using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}