using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement
{
    public interface ILabTestService
    {
        Task<ICollection<LabTest>> GetAllTestAsync();
        Task AddNewTestAsync(LabTestDTO labTest);
        Task DeleteTestAsync(int LabTestId);
    }
}