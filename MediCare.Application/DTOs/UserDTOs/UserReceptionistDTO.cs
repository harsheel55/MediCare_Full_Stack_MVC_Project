using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs
{
    public class UserReceptionistDTO : UserRegisterDTO
    {
        [Required(ErrorMessage = "Qualification is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Qualification must be between 2 and 100 characters.")]
        public string Qualification { get; set; }
    }
}