using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCare_MVC_Project.MediCare.Domain.Entity
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // Cash, Card, UPI

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "AmountPaid must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateOnly PaymentDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property 
        public virtual Invoice Invoice { get; set; }
    }
}