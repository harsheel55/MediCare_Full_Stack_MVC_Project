namespace MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement
{
    public class PatientTestDTO
    {
        public int PatientId { get; set; }
        public int TestId { get; set; }
        public DateOnly TestDate { get; set; }
        public string? Result { get; set; } = null;
    }
}