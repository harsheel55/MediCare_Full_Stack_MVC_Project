using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement
{
    public interface ISpecializationService
    {
        Task<ICollection<GetSpecializationDTO>> GetAllSpecializationAsync();
        Task<GetSpecializationDTO> GetSpecializationById(int id);
        Task AddSpecializationAsync(int createdById, string specializationName);
        Task UpdateSpecializationByIdAsync(int updatedById, int id, string specializationName);
        Task DeleteSpecializationByIdAsync(int id);
    }
}