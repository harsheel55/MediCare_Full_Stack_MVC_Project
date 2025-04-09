using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement
{
    public interface IPatientService
    {
        Task<ICollection<GetPatientDTO>> GetAllPatientAsync();
        Task AddPatientAsync(int id, GetPatientDTO patient);
    }
}