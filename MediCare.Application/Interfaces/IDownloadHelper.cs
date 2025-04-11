using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces
{
    public interface IDownloadHelper
    {
        Task<byte[]> ConvertNoteTextToPdfAsync(PdfNoteDTO getPatientNotes);
    }
}