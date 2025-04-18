using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement
{
    public interface IRoomService
    {
        Task<ICollection<GetRoomDTO>> GetAllRoomAsync();
        Task AddNewRoomAsync(int id, RoomDTO room);
        Task UpdateRoomAsync(int roomId, int roomNo, string roomType);
        Task DeleteRoomAsync(int id);
    }
}