using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddNewRoomAsync(int id, RoomDTO room)
        {
            await _roomRepository.AddNewRoomQuery(id, room);
        }

        public async Task DeleteRoomAsync(int roomNo)
        {
            await _roomRepository.DeleteRoomQuery(roomNo);
        }

        public Task<ICollection<GetRoomDTO>> GetAllRoomAsync()
        {
            return _roomRepository.GetAllRoomQuery();
        }

        public async Task UpdateRoomAsync(int roomId, int roomNo, string roomType)
        {
            await _roomRepository.UpdateRoomQuery(roomId, roomNo, roomType);
        }
    }
}