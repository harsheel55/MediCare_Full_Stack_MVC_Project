using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement
{
    public interface IAdmissionRepository
    {
        Task<ICollection<GetAdmissionDTO>> GetAllAdmissionRecordsQuery();
        Task AddAdmissionRecordsQuery(AdmissionDTO admission);
        Task DeleteAdmissionRecordQuery(int AdmissionId);
        Task UpdateAdmissionRecordQuery(int AdmissionId, AdmissionUpdateDTO admission);
        Task<AdmissionUpdateDTO> GetAdmissionRecordsByIdQuery(int id);
    }
}