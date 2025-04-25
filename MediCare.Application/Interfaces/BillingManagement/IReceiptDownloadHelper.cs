using MediCare_MVC_Project.MediCare.Application.DTOs.BillingDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.BillingManagement
{
    public interface IReceiptDownloadHelper
    {
        Task<byte[]> GenerateMedicalReceiptAsync(ReceiptDTO receipt);
    }
}
