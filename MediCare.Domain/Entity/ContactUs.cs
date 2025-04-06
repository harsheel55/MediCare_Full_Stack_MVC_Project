using System;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class ContactUs
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string QueryType { get; set; }  // New field

        [Required]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}