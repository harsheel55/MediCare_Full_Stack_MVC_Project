using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement
{
    public interface IPatientTestRepository
    {
        Task<ICollection<GetPatientTestDTO>> GetAllPatientTestQuery();
        Task AddPatientTestQuery(PatientTestDTO patientTest);
        Task DeletePatientTestQuery(int patientTestId);
        Task UpdatePatientTestQuery(int patientTestId, DateOnly date, string result);
    }
}