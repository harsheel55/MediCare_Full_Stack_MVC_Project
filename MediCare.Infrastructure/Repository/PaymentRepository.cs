using MediCare_MVC_Project.MediCare.Application.DTOs.PaymentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PaymentManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDBContext context) : base(context)
        {
            
        }

        public async Task AddNewPaymentQuery(PaymentDTO payment)
        {
            var existingRecord = await _context.Payments.Include(s => s.Invoice)
                                                        .FirstOrDefaultAsync(s => s.Invoice.AppointmentId == payment.AppointmentId);

            if (existingRecord != null)
                throw new Exception("Records already exists.");

            
        }

        public async Task DeletePaymentQuery(int PaymentId)
        {
            var existingRecords = await _context.Payments.FindAsync(PaymentId);
            if (existingRecords == null)
                throw new Exception("No records found.");

            _context.Payments.Remove(existingRecords);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetPaymentDTO>> GetAllPaymentQuery()
        {
            var paymentRecords = await _context.Payments.Include(s => s.Invoice)
                                                            .ThenInclude(s => s.Appointment)
                                                            .ThenInclude(s => s.Patient)
                                                            .Select(r => new GetPaymentDTO
                                                            {
                                                                PaymentId = r.PaymentId,
                                                                FirstName = r.Invoice.Appointment.Patient.FirstName,
                                                                LastName = r.Invoice.Appointment.Patient.LastName,
                                                                Email = r.Invoice.Appointment.Patient.Email,
                                                                MobileNo = r.Invoice.Appointment.Patient.MobileNo,
                                                                AppointmentId = r.Invoice.AppointmentId,
                                                                AmoundPaid = r.AmountPaid,
                                                                Amount = r.Invoice.Amount,
                                                                PaymentStatus = r.Invoice.PaymentStatus,
                                                                PaymentMethod = r.PaymentMethod,
                                                                PaymentDate = r.PaymentDate
                                                            }).ToListAsync();

            if (paymentRecords == null)
                throw new Exception("No records found.");

            return paymentRecords;
        }
    }
}