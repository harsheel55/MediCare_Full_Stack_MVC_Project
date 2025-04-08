using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement
{
    public interface IDoctorService
    {
        Task<ICollection<GetDoctorDTO>> GetAllDoctorAsync();
        //Task<GetDoctorDTO> GetDoctorByIdAsync(int id);
        Task AddDoctorAsync(int id, UserDoctorDTO doctor);
        //Task UpdateDoctorAsync(int updatedById, int id, DoctorDTO patient);
        //Task DeleteDoctorAsync(int id);
    }
}