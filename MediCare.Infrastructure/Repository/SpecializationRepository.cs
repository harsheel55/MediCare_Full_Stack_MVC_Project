using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task AddSpecializationQuery(int createdById, string specializationName)
        {
            try
            {
                var existingSpecialization = await _context.Specializations.FirstOrDefaultAsync(s => s.SpecializationName == specializationName);

                if (existingSpecialization != null)
                    throw new ArgumentException("Specialization is already exist.");

                var newSpecialization = new Specialization
                {
                    SpecializationName = specializationName,
                    CreatedBy = createdById,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Specializations.Add(newSpecialization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteSpecializationByIdQuery(int id)
        {
            try
            {
                var existingSpecialization = await _context.Specializations.FindAsync(id);

                if (existingSpecialization == null)
                    throw new ArgumentException("Specialization is not found");

                _context.Specializations.Remove(existingSpecialization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<GetSpecializationDTO>> GetAllSpecializationQuery()
        {
            try
            {
                var specializationsList = await _context.Specializations.Select(s => new GetSpecializationDTO
                {
                    SpecializationId = s.SpecializationId,
                    SpecializationName = s.SpecializationName,
                    TotalDoctors = s.Doctors.Count
                }).ToListAsync();

                return specializationsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetSpecializationDTO> GetSpecializationByIdQuery(int id)
        {
            try
            {
                var specialization = await _context.Specializations.Where(s => s.SpecializationId == id)
                                                                   .Select(s => new GetSpecializationDTO
                                                                   {
                                                                       SpecializationId = s.SpecializationId,
                                                                       SpecializationName = s.SpecializationName,
                                                                       TotalDoctors = s.Doctors.Count
                                                                   }).FirstOrDefaultAsync();

                if (specialization == null)
                    throw new KeyNotFoundException("No Specialization found.");

                return specialization;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateSpecializationByIdQuery(int updatedById, int id, string specializationName)
        {
            try
            {
                var existingSpecialization = await _context.Specializations.FindAsync(id);

                if (existingSpecialization == null)
                    throw new ArgumentException("Specialization is not found");

                existingSpecialization.SpecializationName = specializationName ?? existingSpecialization.SpecializationName;
                existingSpecialization.UpdatedBy = updatedById;
                existingSpecialization.UpdatedAt = DateTime.UtcNow;

                _context.Specializations.Update(existingSpecialization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}