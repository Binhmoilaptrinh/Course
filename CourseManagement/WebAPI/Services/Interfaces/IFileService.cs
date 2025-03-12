using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<BlobDto>> ListAsync();
        Task<BlobResponseDto> UploadAsync(IFormFile blob);
        public byte[] GenerateCertificatePdf(string userName, string courseName, DateTime issueDate);
        Task<Certificate> GenerateAndUploadCertificateAsync(CertificateRequest cer);
    }
}
