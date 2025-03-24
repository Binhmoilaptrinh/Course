
﻿using Azure.Storage;
using Azure.Storage.Blobs;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
﻿using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;
using System.Xml.Linq;
using WebAPI.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using PdfSharp.Fonts;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;

namespace WebAPI.Services
{
    public class FileService : IFileService
    {
        private readonly string _storageAccount = "elearningcourse";
        private readonly string _key = "d7y4wTEjIl0mMyy9Tlu6Ds9qgS0twnsKblHJ+aexF6PPg/0IoCk+cpT3QSMBazoU/0C+e4krt9cr+AStz43GaA==";
        private BlobContainerClient _filesContainer;
        private readonly ECourseContext _context;

        public FileService(ECourseContext context)
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("web");
            _context = context;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            List<BlobDto> files = new List<BlobDto>();
            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}.{name}";
                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);
            await using (Stream data = blob.OpenReadStream())
            {
                await client.UploadAsync(data, overwrite: true);
            }
            response.Status = $"File {blob.FileName} upload successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
        public byte[] GenerateCertificatePdf(string userName, string courseName, DateTime issueDate)
        {
            // Create a new document
            Document document = new Document();
            Section section = document.AddSection();

            // Set page size and margins
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.TopMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1.5);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1.5);

            // 🎨 Create a Border Using a Table
            Table borderTable = section.AddTable();
            borderTable.Borders.Width = 5; // Thick Border
            borderTable.Borders.Color = Colors.Gold;
            borderTable.Borders.Distance = 10; // Adds spacing from content

            borderTable.AddColumn(Unit.FromCentimeter(18)); // Corrected column width

            Row row = borderTable.AddRow();
            Cell cell = row.Cells[0];

            cell.Shading.Color = Colors.White;
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.Format.SpaceBefore = 10;
            cell.Format.SpaceAfter = 10;

            // 🏆 Title (Removed unsupported emoji)
            Paragraph title = cell.AddParagraph("CERTIFICATE OF ACHIEVEMENT");
            title.Format.Font.Size = 32;
            title.Format.Font.Bold = true;
            title.Format.Font.Color = Colors.DarkBlue;
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.SpaceAfter = 15;

            // ✨ Recipient Name
            Paragraph name = cell.AddParagraph(userName);
            name.Format.Font.Size = 26;
            name.Format.Font.Bold = true;
            name.Format.Font.Color = Colors.DarkRed;
            name.Format.Alignment = ParagraphAlignment.Center;
            name.Format.SpaceAfter = 10;

            // 📜 Course Name
            Paragraph course = cell.AddParagraph();
            FormattedText formattedCourse = course.AddFormattedText($"has successfully completed the course:\n\"{courseName}\"", TextFormat.Italic);
            course.Format.Font.Size = 20;
            course.Format.Alignment = ParagraphAlignment.Center;
            course.Format.SpaceAfter = 15;

            // 📅 Issue Date
            Paragraph date = cell.AddParagraph($"Date Issued: {issueDate:dd MMMM yyyy}");
            date.Format.Font.Size = 16;
            date.Format.Alignment = ParagraphAlignment.Center;
            date.Format.SpaceAfter = 20;

            // ✍ Signature (Added more spacing)
            Paragraph signature = cell.AddParagraph("__________________________");
            signature.Format.Font.Size = 14;
            signature.Format.Alignment = ParagraphAlignment.Right;
            signature.Format.SpaceBefore = 20;
            signature.Format.SpaceAfter = 5;

            // ✍ Signature Name
            Paragraph signatureName = cell.AddParagraph("Phạm Thanh Bình");
            signatureName.Format.Font.Size = 16;
            signatureName.Format.Font.Bold = true;
            signatureName.Format.Font.Color = Colors.DarkSlateBlue;
            signatureName.Format.Alignment = ParagraphAlignment.Right;
            signatureName.Format.SpaceAfter = 5;

            // ✍ Designation (Italicized)
            Paragraph designation = cell.AddParagraph("Course Director");
            designation.Format.Font.Size = 14;
            designation.Format.Font.Italic = true;
            designation.Format.Alignment = ParagraphAlignment.Right;
            designation.Format.SpaceAfter = 15;

            // Render the PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();

            // Save to memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                renderer.PdfDocument.Save(stream, false);
                return stream.ToArray();
            }
        }






        public async Task<Certificate> GenerateAndUploadCertificateAsync(CertificateRequest cer)
        {
            // Step 1: Generate the Certificate PDF
            string certificateName = $"Certificate_{cer.EnrollmentID}_{DateTime.UtcNow.Ticks}.pdf";
            byte[] pdfBytes = GenerateCertificatePdf(cer.UserName, cer.CourseName, DateTime.UtcNow);
            var enrollment = _context.Enrollments.FirstOrDefault(x => x.Id == cer.EnrollmentID);

            enrollment.Status = 3;
            await _context.SaveChangesAsync();
            // Step 2: Upload the PDF to Azure Blob Storage
            BlobClient client = _filesContainer.GetBlobClient(certificateName);
            await using (MemoryStream ms = new MemoryStream(pdfBytes))
            {
                await client.UploadAsync(ms, overwrite: true);
            }

            // Step 3: Save the Certificate record to the database
            Certificate certificate = new Certificate
            {
                EnrollmentId = cer.EnrollmentID,
                IssueDate = DateTime.UtcNow,
                CertificateNumber = Guid.NewGuid().ToString(), // Unique number for the certificate
                CertificateUrl = client.Uri.AbsoluteUri
            };

            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();

            return certificate;
        }

    }
}