using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement
{
    public interface ICheckUpService
    {
        Task<ICollection<GetCheckUpDTO>> GetAllCheckUpAsync();
        Task<CheckUpDTO> GetCheckupFormDataAsync(int id);
        Task<bool> AddPatientNoteAsync(CheckUpDTO patientNoteView, int id);

        Task DeletePatientNotesAsync(int id);
    }
}
