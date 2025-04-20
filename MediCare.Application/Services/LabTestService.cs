using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly ILabTestRepository _labTestRepository;

        public LabTestService(ILabTestRepository labTestRepository)
        {
            _labTestRepository = labTestRepository;
        }

        public async Task AddNewTestAsync(LabTestDTO labTest)
        {
            await _labTestRepository.AddNewTestQuery(labTest);
        }

        public async Task DeleteTestAsync(int LabTestId)
        {
            await _labTestRepository.DeleteTestQuery(LabTestId);
        }

        public async Task<ICollection<LabTest>> GetAllTestAsync()
        {
            return await _labTestRepository.GetAllTestQuery();
        }

        public async Task UpdateLabTestAsync(int TestId, string TestName, string Description, decimal cost)
        {
            await _labTestRepository.UpdateLabTestQuery(TestId, TestName, Description, cost);
        }
    }
}