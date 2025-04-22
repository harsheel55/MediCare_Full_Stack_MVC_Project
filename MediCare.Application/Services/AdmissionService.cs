using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionRepository _admissionRepository;

        public AdmissionService(IAdmissionRepository admissionRepository)
        {
            _admissionRepository = admissionRepository;
        }

        public async Task AddAdmissionRecordsAsync(AdmissionDTO admission)
        {
            await _admissionRepository.AddAdmissionRecordsQuery(admission);
        }

        public async Task DeleteAdmissionRecordAsync(int AdmissionId)
        {
            await _admissionRepository.DeleteAdmissionRecordQuery(AdmissionId);
        }

        public async Task<AdmissionUpdateDTO> GetAdmissionRecordsByIdAsync(int id)
        {
            return await _admissionRepository.GetAdmissionRecordsByIdQuery(id);
        }

        public async Task<ICollection<GetAdmissionDTO>> GetAllAdmissionByDoctorAsync(int id)
        {
            return await _admissionRepository.GetAllAdmissionByDoctorQuery(id);
        }

        public async Task<ICollection<GetAdmissionDTO>> GetAllAdmissionRecordsAsync()
        {
            return await _admissionRepository.GetAllAdmissionRecordsQuery();
        }

        public async Task UpdateAdmissionRecordAsync(int AdmissionId, AdmissionUpdateDTO admission)
        {
            await _admissionRepository.UpdateAdmissionRecordQuery(AdmissionId, admission);
        }
    }
}