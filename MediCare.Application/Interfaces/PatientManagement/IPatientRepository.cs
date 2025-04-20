using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement
{
    public interface IPatientRepository
    {
        Task<ICollection<GetPatientDTO>> GetAllPatientQuery();
        Task AddPatientQuery(int id, GetPatientDTO receptionist);
        Task DeletePatientQuery(int id);
        Task<GetPatientDTO> GetPatientByIdQuery(int id);
        Task UpdatePatientQuery(int id, GetPatientDTO getPatient, int updatedBy);
    }
}