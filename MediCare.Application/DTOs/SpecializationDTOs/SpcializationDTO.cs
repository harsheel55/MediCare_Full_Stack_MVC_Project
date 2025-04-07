using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs
{
    public class SpecializationDTO
    {
        [Required(ErrorMessage = "Specialization name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Specialization name must be between 3 and 100 characters.")]
        public string SpecializationName { get; set; }
    }
}