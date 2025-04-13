using MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.PaymentManagement
{
    public interface IPaymentService
    {
        Task<ICollection<GetPaymentDTO>> GetAllPaymentAsync();
        Task AddNewPaymentAsync(PaymentDTO payment);
        Task DeletePaymentAsync(int PaymentId);
    }
}