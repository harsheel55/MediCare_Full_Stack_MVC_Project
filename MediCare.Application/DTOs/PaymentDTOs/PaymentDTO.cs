using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs
{
    public class PaymentDTO
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public int InvoiceId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}