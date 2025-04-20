using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class BedService : IBedService
    {
        private readonly IBedRepository _bedRepository;

        public BedService(IBedRepository bedRepository)
        {
            _bedRepository = bedRepository;
        }

        public async Task AddNewBedAsync(BedDTO bed)
        {
            await _bedRepository.AddNewBedQuery(bed);
        }

        public async Task DeleteBedAsync(int BedId)
        {
            await _bedRepository.DeleteBedQuery(BedId);
        }

        public async Task<ICollection<GetBedDTO>> GetAllBedsAsync()
        {
            return await _bedRepository.GetAllBedsQuery();
        }

        public async Task UpdateBedAsync(int BedId, int BedNo, int RoomNo, string RoomType, bool IsOccupied)
        {
            await _bedRepository.UpdateBedQuery(BedId, BedNo, RoomNo, RoomType, IsOccupied);
        }
    }
}
