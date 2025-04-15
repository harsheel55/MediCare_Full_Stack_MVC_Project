namespace MediCare_MVC_Project.Models
{
    public class BedViewModel
    {
        public int BedId { get; set; }
        public int BedNo { get; set; }
        public int RoomNo { get; set; }
        public string RoomType { get; set; }
        public bool IsOccupied { get; set; }
    }
}