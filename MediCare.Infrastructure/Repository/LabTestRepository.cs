using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class LabTestRepository : GenericRepository<LabTest>, ILabTestRepository
    {
        public LabTestRepository(ApplicationDBContext context) : base(context)
        {
            
        }
        public async Task AddNewTestQuery(LabTestDTO labTest)
        {
            var existingRecord = await _context.LabTests.FirstOrDefaultAsync(s => s.TestName == labTest.TestName);

            if (existingRecord != null)
                throw new Exception("Lab Test already registered.");

            var newTest = new LabTest
            {
                TestName = labTest.TestName,
                Description = labTest.Description,
                Cost = labTest.Cost,
                CreatedAt = DateTime.UtcNow
            };

            _context.LabTests.Add(newTest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTestQuery(int LabTestId)
        {
            var existingRecord = await _context.LabTests.FindAsync(LabTestId);

            if (existingRecord == null)
                throw new Exception("No records found.");

            _context.LabTests.Remove(existingRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<LabTest>> GetAllTestQuery()
        {
            var testsList = await _context.LabTests.Select(s => new LabTest
            {
                TestId = s.TestId,
                TestName = s.TestName,
                Description = s.Description,
                Cost = s.Cost,
            })
            .OrderBy(s => s.TestName)
            .ThenBy(s => s.Cost)
            .ToListAsync();

            if (testsList == null)
                throw new Exception("No Data found.");

            return testsList;
        }

        public async Task UpdateLabTestQuery(int TestId, string TestName, string Description, decimal cost)
        {
            var existingTest = await _context.LabTests.FindAsync(TestId);

            if (existingTest == null)
                throw new Exception($"No Lab Test found with {TestId}");

            existingTest.TestName = TestName;
            existingTest.Description = Description;
            existingTest.Cost = cost;

            _context.LabTests.Update(existingTest);
            await _context.SaveChangesAsync();
        }
    }
}
