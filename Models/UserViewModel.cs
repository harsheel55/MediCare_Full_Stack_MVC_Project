namespace MediCare_MVC_Project.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string EmergencyNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }
}