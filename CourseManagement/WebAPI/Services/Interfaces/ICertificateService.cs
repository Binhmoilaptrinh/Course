using WebAPI.DTOS.request;

namespace WebAPI.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<string> GetCertificateUrl(int enrollmentId);
    }
}
