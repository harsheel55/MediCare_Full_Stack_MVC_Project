namespace MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs
{
    public class GetRoomDTO
    {
        public int RoomId { get; set; }
        public int RoomNo { get; set; }
        public string RoomType { get; set; }
        public int TotalBeds { get; set; }
    }
}