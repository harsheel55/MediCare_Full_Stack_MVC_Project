
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces
{
    public interface IEmailHelper
    {
        public Task SendUserQueryAcknowledgementEmailAsync(ContactUsDTO contactUs);
        public Task SendEmailAsync(string toEmail, string subject, string body, byte[]? attachmentBytes = null, string? attachmentName = null);
        public Task SendUserRegistrationEmailAsync(string toEmail, string? password);
        public Task SendPatientRegistrationEmailAsync(GetPatientDTO patient);
        //public Task SendAppointmentStatusEmailAsync(string patientEmail, string doctorName, Appointment appointment);
        //public Task SendPatientNotesEmailAsync(string patientEmail, PdfNotesDTO pdfNotesDTO, byte[] PdfBytes);
    }
}