using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement
{
    public interface IRoomRepository
    {
        Task<ICollection<GetRoomDTO>> GetAllRoomQuery();
        Task AddNewRoomQuery(int id, RoomDTO room);
        Task UpdateRoomQuery(int roomId, int roomNo, string roomType);
        Task DeleteRoomQuery(int id);
    }
}