using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task AddUserAsync(int id, UserRegisterDTO userDto);
        Task AddDoctorAsync(int id, UserDoctorDTO userDoctor);
        Task AddReceptionistAsync(int id, UserReceptionistDTO userReceptionist);
        Task UpdateUserAsync(int updatedById, int id, UserRegisterDTO user);
        Task DeleteUserAsync(int id);
    }
}