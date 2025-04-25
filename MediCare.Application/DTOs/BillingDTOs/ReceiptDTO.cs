namespace MediCare_MVC_Project.MediCare.Application.DTOs.BillingDTOs
{
    public class ReceiptDTO
    {
        public string ReceiptNumber { get; set; }
        public string PatientName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string AadharNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DoctorName { get; set; }
        public string AppointmentDescription { get; set; }
        public List<ServiceItem> Services { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceDue { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
