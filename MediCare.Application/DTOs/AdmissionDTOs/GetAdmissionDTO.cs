namespace MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs
{
    public class GetAdmissionDTO
    {
        public int AdmissionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string RoomType { get; set; }
        public int RoomNo { get; set; }
        public int BedNo { get; set; }
        public DateOnly AdmissionDate { get; set; }
        public DateOnly? DischargeDate { get; set; }
    }
}