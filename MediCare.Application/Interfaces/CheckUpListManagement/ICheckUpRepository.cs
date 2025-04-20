using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement
{
    public interface ICheckUpRepository
    {
        Task<ICollection<GetCheckUpDTO>> GetAllCheckUpQuery();
        Task<CheckUpDTO> GetCheckupFormDataQuery(int id);
        Task<bool> AddPatientNoteQuery(CheckUpDTO patientNoteView, int id);
        Task UpdateCheckupNotesQuery(int noteId, string notes);
        Task DeletePatientNotesQuery(int id);
        Task<bool> SendPatientNotePdfQuery(int id);
        Task<byte[]> DownloadNotesPdfQuery(int id);
        Task<PdfNoteDTO> GetNotesData(int id);
    }
}