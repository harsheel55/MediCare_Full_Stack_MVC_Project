using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<ICollection<UserDTO>> GetAllUsersQuery();
        Task<UserDTO> GetUserByIdQuery(int id);
        Task AddUserQuery(int id, UserRegisterDTO userDto);
        Task AddDoctorQuery(int id, UserDoctorDTO userDoctor);
        Task AddReceptionistQuery(int id, UserReceptionistDTO userReceptionist);
        Task UpdateUserQuery(int updatedById, int id, UserRegisterDTO user);
        Task DeleteUserQuery(int id);
    }
}