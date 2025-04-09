using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs
{
    public class GetPatientDTO
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AadharNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}