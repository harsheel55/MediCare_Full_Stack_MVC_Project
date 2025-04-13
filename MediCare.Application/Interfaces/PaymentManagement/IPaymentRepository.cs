using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.PaymentManagement
{
    public interface IPaymentRepository
    {
        Task<ICollection<GetPaymentDTO>> GetAllPaymentQuery();
        Task AddNewPaymentQuery(PaymentDTO payment);
        Task DeletePaymentQuery(int PaymentId);
    }
}