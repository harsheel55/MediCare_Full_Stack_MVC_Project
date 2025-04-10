using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class CheckUpService : ICheckUpService
    {
        private readonly ICheckUpRepository _checkUpRepository;

        public CheckUpService(ICheckUpRepository checkUpRepository)
        {
            _checkUpRepository = checkUpRepository;
        }

        public async Task<bool> AddPatientNoteAsync(CheckUpDTO patientNoteView, int id)
        {
            return await _checkUpRepository.AddPatientNoteQuery(patientNoteView, id);
        }

        public async Task DeletePatientNotesAsync(int id)
        {
           await _checkUpRepository.DeletePatientNotesQuery(id);
        }

        public async Task<ICollection<GetCheckUpDTO>> GetAllCheckUpAsync()
        {
            var checkUpList = await _checkUpRepository.GetAllCheckUpQuery();
            return checkUpList;
        }

        public Task<CheckUpDTO> GetCheckupFormDataAsync(int id)
        {
           return _checkUpRepository.GetCheckupFormDataQuery(id);
        }
    }
}
