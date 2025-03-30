
﻿using Azure.Storage;
using Azure.Storage.Blobs;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace WebAPI.Services
{
    public class FileService : IFileService
    {
        private readonly string _storageAccount = "elearningcourse";
        private readonly string _key = "d7y4wTEjIl0mMyy9Tlu6Ds9qgS0twnsKblHJ+aexF6PPg/0IoCk+cpT3QSMBazoU/0C+e4krt9cr+AStz43GaA==";
        private BlobContainerClient _filesContainer;
        private readonly ECourseContext _context;
        private readonly Cloudinary _cloudinary;

        public FileService(ECourseContext context)
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("web");
            _context = context;
            var account = new Account("doslvje9p", "111261246971633", "lMdfdvz3SsDnpHJA_WDBtRQmFKU");
            _cloudinary = new Cloudinary(account);
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


        public async Task<string> AddStudentNameToPdf(string pdfUrl, string studentName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // 📥 Bước 1: Tải PDF từ Cloudinary về MemoryStream
                    byte[] pdfBytes = await client.GetByteArrayAsync(pdfUrl);
                    MemoryStream inputPdfStream = new MemoryStream(pdfBytes);
                    inputPdfStream.Position = 0; // 🔥 Reset vị trí về đầu file

                    using (MemoryStream outputPdfStream = new MemoryStream())
                    {
                        // 📌 Bước 2: Đọc PDF từ MemoryStream
                        PdfReader reader = new PdfReader(inputPdfStream);
                        PdfWriter writer = new PdfWriter(outputPdfStream);
                        PdfDocument pdfDoc = new PdfDocument(reader, writer);

                        // 📌 Bước 3: Chèn nội dung vào PDF
                        PdfPage page = pdfDoc.GetPage(1);
                        PdfCanvas canvas = new PdfCanvas(page);
                        PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                        canvas.BeginText()
                              .SetFontAndSize(font, 18)
                              .MoveText(150, 500)
                              .ShowText(studentName)
                              .EndText();

                        pdfDoc.Close(); // Đóng tài liệu

                        // 📌 Bước 4: Upload lại lên Cloudinary
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription("updated_certificate.pdf", new MemoryStream(outputPdfStream.ToArray())),
                            PublicId = "updated_certificate"
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        return uploadResult.SecureUrl.ToString(); // 📤 Trả về link PDF mới
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
                return null;
            }
        }


        public async Task<Certificate> GenerateAndUploadCertificateAsync(CertificateRequest cer)
        {
            // Step 1: Generate the Certificate PDF
            string certificateName = $"Certificate_{cer.EnrollmentID}_{DateTime.UtcNow.Ticks}.pdf";
            //byte[] pdfBytes = GenerateCertificatePdf(cer.UserName, cer.CourseName, DateTime.UtcNow);
            var enrollment = _context.Enrollments.FirstOrDefault(x => x.Id == cer.EnrollmentID);

            enrollment.Status = 3;
            await _context.SaveChangesAsync();

            var existingTemplate = await _context.CertificateTemplate.FirstOrDefaultAsync();

            Certificate certificate = new Certificate
            {
                EnrollmentId = cer.EnrollmentID,
                IssueDate = DateTime.UtcNow,
                CertificateNumber = Guid.NewGuid().ToString(), // Unique number for the certificate
                CertificateUrl = await AddStudentNameToPdf(existingTemplate.Url, cer.UserName)
            };

            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();

            return certificate;
        }

    }
}