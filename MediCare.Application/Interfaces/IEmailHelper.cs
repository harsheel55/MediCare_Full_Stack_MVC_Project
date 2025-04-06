
namespace MediCare_MVC_Project.MediCare.Application.Interfaces
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[]? attachmentBytes = null, string? attachmentName = null);
        public Task SendUserRegistrationEmailAsync(string toEmail, string? password);
        //public Task SendPatientRegistrationEmailAsync(PatientDTO patient);
        //public Task SendAppointmentStatusEmailAsync(string patientEmail, string doctorName, Appointment appointment);
        //public Task SendPatientNotesEmailAsync(string patientEmail, PdfNotesDTO pdfNotesDTO, byte[] PdfBytes);
    }
}