using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement
{
    public interface ICheckUpRepository
    {
        Task<ICollection<GetCheckUpDTO>> GetAllCheckUpQuery();
        Task<CheckUpDTO> GetCheckupFormDataQuery(int id);
        Task<bool> AddPatientNoteQuery(CheckUpDTO patientNoteView, int id);
        Task DeletePatientNotesQuery(int id);
    }
}