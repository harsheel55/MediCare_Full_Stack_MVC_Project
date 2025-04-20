using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class PatientTestService : IPatientTestService
    {
        private readonly IPatientTestRepository _patientTestRepository;

        public PatientTestService(IPatientTestRepository patientTestRepository)
        {
            _patientTestRepository = patientTestRepository;
        }

        public async Task AddPatientTestAsync(PatientTestDTO patientTest)
        {
            await _patientTestRepository.AddPatientTestQuery(patientTest);
        }

        public async Task DeletePatientTestAsync(int patientTestId)
        {
            await _patientTestRepository.DeletePatientTestQuery(patientTestId);
        }

        public Task<ICollection<GetPatientTestDTO>> GetAllPatientTestAsync()
        {
            return _patientTestRepository.GetAllPatientTestQuery();
        }

        public async Task UpdatePatientTestAsync(int patientTestId, DateOnly date, string result)
        {
            await _patientTestRepository.UpdatePatientTestQuery(patientTestId, date, result);
        }
    }
}