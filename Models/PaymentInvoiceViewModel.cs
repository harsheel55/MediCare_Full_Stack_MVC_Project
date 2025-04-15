namespace MediCare_MVC_Project.Models
{
    public class PaymentInvoiceViewModel
    {
        public int PaymentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public decimal AmoundPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}