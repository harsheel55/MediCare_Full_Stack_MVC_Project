using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs
{
    public class UserDoctorDTO : UserRegisterDTO
    {
        [Required(ErrorMessage = "Specialization ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Specialization ID must be a positive integer.")]
        public int SpecializationId { get; set; }

        [Required(ErrorMessage = "Qualification is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Qualification must be between 3 and 200 characters.")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "License Number is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "License Number must be between 5 and 50 characters.")]
        public string LicenceNumber { get; set; }
    }
}