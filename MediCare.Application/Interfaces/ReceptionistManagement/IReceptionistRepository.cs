using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement
{
    public interface IReceptionistRepository
    {
        Task<ICollection<GetReceptionistDTO>> GetAllReceptionistQuery();
        Task AddReceptionistQuery(int id, UserReceptionistDTO receptionist);
    }
}