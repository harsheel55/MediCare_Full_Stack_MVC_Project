using iTextSharp.text;
using iTextSharp.text.pdf;
using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;

namespace MediCare_MVC_Project.MediCare.Common.Helpers
{
    public class NoteDownloadHelper : IDownloadHelper
    {
        public async Task<byte[]> ConvertNoteTextToPdfAsync(PdfNoteDTO notes)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Fonts
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK);
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);
                Font textFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

                // Logos
                Image medicareLogo = Image.GetInstance("https://drive.google.com/uc?export=view&id=1SVjUvEmC-oxtaBkQ0f8ounCkQ2xMSPAN");
                medicareLogo.ScaleAbsolute(120, 30);
                medicareLogo.SetAbsolutePosition(50, 780);

                Image bacancyLogo = Image.GetInstance("https://drive.google.com/uc?export=view&id=1J-OM4zr7GXGMuI6Hzl6pT8HOsAL98z5n");
                bacancyLogo.ScaleAbsolute(150, 30);
                bacancyLogo.SetAbsolutePosition(400, 780);

                document.Add(medicareLogo);
                document.Add(bacancyLogo);
                document.Add(new Paragraph("\n\n"));

                // Title
                Paragraph title = new Paragraph("Patient Notes", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                document.Add(title);

                // Date Generated
                Paragraph date = new Paragraph("Generated on: " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"), textFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 10f
                };
                document.Add(date);

                // Patient Details
                document.Add(CreateText("Patient Name:", notes.PatientName));
                document.Add(CreateText("Doctor Name:", notes.DoctorName));
                document.Add(CreateText("Appointment Date:", notes.AppointmentDate.ToString("dd-MM-yyyy")));
                document.Add(CreateText("Time:", $"{DateTime.Today.Add(notes.StartTime):hh:mm tt} - {DateTime.Today.Add(notes.EndTime):hh:mm tt}"));
                document.Add(CreateText("Description:", notes.Description));
                document.Add(CreateText("Mobile No:", notes.MobileNo));
                document.Add(CreateText("Aadhar No:", notes.AadharNo));
                document.Add(CreateText("Address:", notes.Address));
                document.Add(CreateText("Date of Birth:", notes.DateOfBirth.ToString("dd-MM-yyyy")));
                document.Add(CreateText("Gender:", notes.Gender));

                // Notes
                document.Add(new Paragraph("\nNote:", headerFont));
                document.Add(new Paragraph(notes.NoteText, textFont));

                // Footer Notes
                document.Add(new Paragraph("\nImportant Notes:", headerFont));
                document.Add(new Paragraph("1. This is a system-generated document.", textFont));
                document.Add(new Paragraph("2. Please verify details before proceeding with treatment.", textFont));
                document.Add(new Paragraph("3. Keep patient confidentiality in mind.", textFont));

                document.Close();
                writer.Close();

                return memoryStream.ToArray();
            }
        }

        private Paragraph CreateText(string label, string value)
        {
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

            Chunk labelChunk = new Chunk(label + " ", boldFont);
            Chunk valueChunk = new Chunk(value, normalFont);

            Paragraph paragraph = new Paragraph();
            paragraph.Add(labelChunk);
            paragraph.Add(valueChunk);
            paragraph.SpacingAfter = 3f;

            return paragraph;
        }
    }
}