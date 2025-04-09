using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IReceptionistRepository _receptionistRepository;

        public ReceptionistService(IReceptionistRepository receptionistRepository)
        {
            _receptionistRepository = receptionistRepository;
        }

        public Task AddReceptionistAsync(int id, UserReceptionistDTO receptionist)
        {
            return _receptionistRepository.AddReceptionistQuery(id, receptionist);
        }

        public async Task<ICollection<GetReceptionistDTO>> GetAllReceptionistAsync()
        {
            var receptionistList = await _receptionistRepository.GetAllReceptionistQuery();
            return receptionistList;
        }
    }
}