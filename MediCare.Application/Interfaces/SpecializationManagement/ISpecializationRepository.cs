using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement
{
    public interface ISpecializationRepository
    {
        Task<ICollection<GetSpecializationDTO>> GetAllSpecializationQuery();
        Task<GetSpecializationDTO> GetSpecializationByIdQuery(int id);
        Task AddSpecializationQuery(int createdById, string specializationName);
        Task UpdateSpecializationByIdQuery(int updatedById, int id, string specializationName);
        Task DeleteSpecializationByIdQuery(int id);
    }
}