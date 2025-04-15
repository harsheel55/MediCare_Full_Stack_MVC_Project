using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDBContext context) : base(context)
        {
            
        }

        public async Task AddNewRoomQuery(int id, RoomDTO room)
        {
            var existingRecord = await _context.Rooms.FindAsync(room.RoomNo);

            if (existingRecord != null)
                throw new Exception("Room is already registered.");

            var newRoom = new Room
            {
                RoomNumber = room.RoomNo,
                RoomType = room.RoomType,
                IsOccupied = false,
                CreatedBy = id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rooms.Add(newRoom);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetRoomDTO>> GetAllRoomQuery()
        {
            var recordsList = await _context.Rooms
                .Include(r => r.Beds)
                .GroupBy(r => new { r.RoomId, r.RoomNumber, r.RoomType })
                .Select(g => new GetRoomDTO
                {
                    RoomId = g.Key.RoomId,
                    RoomNo = g.Key.RoomNumber,       
                    RoomType = g.Key.RoomType,
                    TotalBeds = g.SelectMany(r => r.Beds).Count()
                })
                .ToListAsync();

            if (recordsList == null)
                throw new Exception("No Records found.");

            return recordsList;
        }
    }
}