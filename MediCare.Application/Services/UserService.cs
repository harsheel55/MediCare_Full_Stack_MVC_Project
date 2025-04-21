using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task AddDoctorAsync(int id, UserDoctorDTO userDoctor)
        {
            return _userRepository.AddDoctorQuery(id, userDoctor);
        }

        public Task AddReceptionistAsync(int id, UserReceptionistDTO userReceptionist)
        {
            return _userRepository.AddReceptionistQuery(id, userReceptionist);
        }

        public Task AddUserAsync(int id, UserRegisterDTO userDto)
        {
            return _userRepository.AddUserQuery(id, userDto);
        }

        public Task DeleteUserAsync(string email)
        {
            return _userRepository.DeleteUserQuery(email);
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersQuery();
        }

        public Task<int> GetDoctorsIdAsync(int userId)
        {
            return _userRepository.GetDoctorsIdQuery(userId);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdQuery(userId);
        }

        public Task UpdateUserAsync(int updatedById, int id, UserRegisterDTO user)
        {
            return _userRepository.UpdateUserQuery(updatedById, id, user);
        }
    }

}
