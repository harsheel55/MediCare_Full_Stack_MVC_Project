using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class BedRepository : GenericRepository<Bed>, IBedRepository
    {
        public BedRepository(ApplicationDBContext context) : base(context)
        {
            
        }
        public async Task AddNewBedQuery(BedDTO bed)
        {
            var existingRecord = await _context.Beds.FirstOrDefaultAsync(s => s.BedNumber == bed.BedNo && s.Room.RoomNumber == bed.RoomNo);

            if (existingRecord != null)
                throw new Exception("Bed already Exists.");

            var findRoom = await _context.Rooms.FirstOrDefaultAsync(s => s.RoomNumber == bed.RoomNo);

            if (findRoom == null)
                throw new Exception("No room found.");

            var newBed = new Bed
            {
                BedNumber = bed.BedNo,
                RoomId = findRoom.RoomId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Beds.Add(newBed);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBedQuery(int BedId)
        {
            var existingBed = await _context.Beds.FindAsync(BedId);


            if (existingBed == null)
                throw new Exception("No Bed found.");

            //existingBed.IsOccupied = false;

            _context.Beds.Remove(existingBed);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetBedDTO>> GetAllBedsQuery()
        {
            var BedList = await _context.Beds.Include(r => r.Room)
                                             .Select(s => new GetBedDTO
                                             {
                                                 BedId = s.BedId,
                                                 RoomNo = s.Room.RoomNumber,
                                                 BedNo = s.BedNumber,
                                                 RoomType = s.Room.RoomType,
                                                 IsOccupied = s.IsOccupied
                                             }).ToListAsync();

            if (BedList == null)
                throw new Exception("No records found.");

            return BedList;
        }
    }
}