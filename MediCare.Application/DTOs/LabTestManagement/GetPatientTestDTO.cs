namespace MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement
{
    public class GetPatientTestDTO
    {
        public int PatientTestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }
        public DateOnly TestDate { get; set; }
        public decimal Cost { get; set; }
        public string Result { get; set; } = null;
    }
}