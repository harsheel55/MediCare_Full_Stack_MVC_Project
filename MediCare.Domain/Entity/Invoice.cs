using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentStatus { get; set; } // Paid, Pending, Failed

        [Url(ErrorMessage = "Invalid URL format.")]
        public string InvoiceUrl { get; set; } // Azure Blob Storage URL

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Appointment Appointment { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
