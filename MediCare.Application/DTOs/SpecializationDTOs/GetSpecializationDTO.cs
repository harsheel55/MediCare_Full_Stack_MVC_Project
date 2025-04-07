namespace MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs
{
    public class GetSpecializationDTO
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public int TotalDoctors { get; set; }
    }
}