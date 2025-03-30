using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<CertificateUserRes> GetCertificate(int enrollmentId);
        Task<CertificateTemplateRes> GetCertificateTemplateUrl();
        Task<CertificateTemplateRes> UploadCertificateUrl(IFormFile filePdf);
    }
}
