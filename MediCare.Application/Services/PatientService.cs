using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Task AddPatientAsync(int id, GetPatientDTO patient)
        {
            return _patientRepository.AddPatientQuery(id, patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            await _patientRepository.DeletePatientQuery(id);
        }

        public Task<ICollection<GetPatientDTO>> GetAllPatientAsync()
        {
            var patientList = _patientRepository.GetAllPatientQuery();
            return patientList;
        }
        public async Task<GetPatientDTO> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetPatientByIdQuery(id);
        }

        public Task<ICollection<GetPatientDTO>> GetPatientsByDoctorIdAsync(int doctorId)
        {
            return _patientRepository.GetPatientsByDoctorIdQuery(doctorId);
        }

        public async Task UpdatePatientAsync(int id, GetPatientDTO getPatient, int updatedBy)
        {
            await _patientRepository.UpdatePatientQuery(id, getPatient, updatedBy);
        }
    }
}