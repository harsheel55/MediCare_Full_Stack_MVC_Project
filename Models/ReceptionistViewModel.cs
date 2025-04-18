using MediCare_MVC_Project.MediCare.Domain.Entity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MediCare_MVC_Project.Models
{
    public class ReceptionistViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public bool Status { get; set; }
        public string StatusText => Status ? "Active" : "Inactive";
    }
}