namespace MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs
{
    public class AdmissionDTO
    {
        public string AadharNo { get; set; }
        public int BedId { get; set; }
        public DateOnly AdmissionDate { get; set; }
        public DateOnly DischargeDate { get; set; }
        public bool IsDischarged { get; set; }
    }
}