using MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PaymentManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task AddNewPaymentAsync(PaymentDTO payment)
        {
            await _paymentRepository.AddNewPaymentQuery(payment);
        }

        public async Task DeletePaymentAsync(int PaymentId)
        {
            await _paymentRepository.DeletePaymentQuery(PaymentId);
        }

        public Task<ICollection<GetPaymentDTO>> GetAllPaymentAsync()
        {
            return _paymentRepository.GetAllPaymentQuery();
        }
    }
}