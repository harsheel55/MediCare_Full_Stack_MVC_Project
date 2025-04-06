using MediCare_MVC_Project.MediCare.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Database
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientAdmission> PatientAdmissions { get; set; }
        public DbSet<PatientNote> PatientNotes { get; set; }
        public DbSet<PatientTest> PatientTests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ---------------------------- Relationship of Tables  ---------------------------- 
            // Role - User (One-to-Many)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Doctor (One-to-One)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Receptionist (One-to-One)
            modelBuilder.Entity<Receptionist>()
                .HasOne(r => r.User)
                .WithOne(u => u.Receptionist)
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Specialization - Doctor (One-to-Many)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - PatientNote (One-to-One)
            modelBuilder.Entity<PatientNote>()
                .HasOne(pn => pn.Appointment)
                .WithOne(a => a.PatientNote)
                .HasForeignKey<PatientNote>(pn => pn.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment - Invoice (One-to-One)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Appointment)
                .WithOne(a => a.Invoice)
                .HasForeignKey<Invoice>(i => i.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice - Payment (One-to-One)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithOne(i => i.Payment)
                .HasForeignKey<Payment>(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room - Bed (One-to-Many)
            modelBuilder.Entity<Bed>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Beds)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bed - PatientAdmission (One-to-One)
            modelBuilder.Entity<PatientAdmission>()
                .HasOne(pa => pa.Bed)
                .WithOne(b => b.PatientAdmission)
                .HasForeignKey<PatientAdmission>(pa => pa.BedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient - PatientAdmission (One-to-One)
            modelBuilder.Entity<PatientAdmission>()
                .HasOne(pa => pa.Patient)
                .WithOne(p => p.PatientAdmission)
                .HasForeignKey<PatientAdmission>(pa => pa.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // LabTest - PatientTest (One-to-Many)
            modelBuilder.Entity<PatientTest>()
                .HasOne(pt => pt.LabTest)
                .WithMany(lt => lt.PatientTests)
                .HasForeignKey(pt => pt.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient - PatientTest (One-to-Many)
            modelBuilder.Entity<PatientTest>()
                .HasOne(pt => pt.Patient)
                .WithMany(p => p.PatientTests)
                .HasForeignKey(pt => pt.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------- Check Constraints  ---------------------------- 
            modelBuilder.Entity<Payment>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Payment_Method", "PaymentMethod IN ('Cash', 'Card', 'UPI', 'Net Banking', 'Insurance', 'Other')"));

            modelBuilder.Entity<Invoice>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Payment_Status", "PaymentStatus IN ('Pending', 'Completed', 'Failed', 'Refunded', 'Partial', 'Cancelled', 'Processing', 'Overdue')"));

            modelBuilder.Entity<Room>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Room_Type", "RoomType IN ('General', 'ICU', 'Private', 'Semi-Private', 'NICU', 'PICU', 'Deluxe', 'Isolation', 'Emergency')"));

            modelBuilder.Entity<Appointment>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Appointment_Status", "Status IN ('Scheduled', 'CheckedIn', 'InProgress', 'Completed', 'Cancelled', 'NoShow', 'Rescheduled', 'PendingApproval', 'Rejected')"));

            modelBuilder.Entity<ContactUs>()
             .ToTable(tb => tb.HasCheckConstraint("CK_ContactUs_QueryType_ValidValues", "QueryType IN ('General Inquiry', 'Appointment', 'Billing', 'Feedback', 'Other')"));


            // ---------------------------- Data Seeding for role table  ---------------------------- 
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Administrator" },
                new Role { RoleId = 2, RoleName = "Doctor" },
                new Role { RoleId = 3, RoleName = "Receptionist" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}