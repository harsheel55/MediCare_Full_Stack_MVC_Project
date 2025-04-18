using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public Task AddDoctorAsync(int id, UserDoctorDTO doctor)
        {
            return _doctorRepository.AddDoctorQuery(id, doctor);
        }

        public async Task DeleteDoctorAsync(string email)
        {
            await _doctorRepository.DeleteDoctorQuery(email);
        }

        public async Task<ICollection<GetDoctorDTO>> GetAllDoctorAsync()
        {
            var doctorList = await _doctorRepository.GetAllDoctorQuery();
            return doctorList;
        }

        public Task<UserDoctorDTO> GetDoctorByEmailAsync(string email)
        {
            return _doctorRepository.GetDoctorByEmailQuery(email);
        }

        public async Task UpdateDoctorAsync(string email, UserDoctorDTO updateDoctor, int updatedById)
        {
            await _doctorRepository.UpdateDoctorQuery(email, updateDoctor, updatedById);
        }
    }
}