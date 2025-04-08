using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement
{
    public interface IReceptionistService
    {
        Task<ICollection<GetReceptionistDTO>> GetAllReceptionistAsync();
        Task AddReceptionistAsync(int id, UserReceptionistDTO receptionist);
    }
}