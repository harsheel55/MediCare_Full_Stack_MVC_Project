using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement
{
    public interface ILabTestRepository
    {
        Task<ICollection<LabTest>> GetAllTestQuery();
        Task AddNewTestQuery(LabTestDTO labTest);
        Task DeleteTestQuery(int LabTestId);
    }
}