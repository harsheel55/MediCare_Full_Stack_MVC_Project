using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement
{
    public interface IDoctorRepository
    {
        Task<ICollection<GetDoctorDTO>> GetAllDoctorQuery();
        //Task<GetDoctorDTO> GetDoctorByIdQuery(int id);
        Task AddDoctorQuery(int id, UserDoctorDTO patient);
        //Task UpdateDoctorQuery(int updatedById, int id, DoctorDTO patient);
        //Task DeleteDoctorQuery(int id);
    }
}
