using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs
{
    public class ContactUsDTO
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string QueryType { get; set; }  // New field

        [Required]
        public string Message { get; set; }
    }
}
