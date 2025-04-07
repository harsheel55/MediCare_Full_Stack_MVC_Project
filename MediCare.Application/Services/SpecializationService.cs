using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationService(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        public Task AddSpecializationAsync(int createdById, string specializationName)
        {
            return _specializationRepository.AddSpecializationQuery(createdById, specializationName);
        }

        public Task DeleteSpecializationByIdAsync(int deptId)
        {
            return _specializationRepository.DeleteSpecializationByIdQuery(deptId);
        }

        public async Task<ICollection<GetSpecializationDTO>> GetAllSpecializationAsync()
        {
            return await _specializationRepository.GetAllSpecializationQuery();
        }

        public async Task<GetSpecializationDTO> GetSpecializationById(int deptId)
        {
            return await _specializationRepository.GetSpecializationByIdQuery(deptId);
        }

        public Task UpdateSpecializationByIdAsync(int updatedById, int deptId, string specializationName)
        {
            return _specializationRepository.UpdateSpecializationByIdQuery(updatedById, deptId, specializationName);
        }
    }

}
