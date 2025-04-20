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

        public async Task<byte[]> DownloadNotesPdfAsync(int id)
        {
            return await _checkUpRepository.DownloadNotesPdfQuery(id);
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

        public async Task<bool> SendPatientNotePdfAsync(int id)
        {
            await _checkUpRepository.SendPatientNotePdfQuery(id);
            return true;
        }

        public async Task UpdateCheckupNotesAsync(int noteId, string notes)
        {
            await _checkUpRepository.UpdateCheckupNotesQuery(noteId, notes);
        }
    }
}
