using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ECourseContext _eCourseContext;
        public CertificateService(ECourseContext eCourseContext)
        {
            _eCourseContext = eCourseContext;
        }
        public async Task<string> GetCertificateUrl(int enrollmentId)
        {
            var certificate = await _eCourseContext.Certificates
                        .Where(c => c.EnrollmentId == enrollmentId)
                        .Select(c => c.CertificateUrl) // Assuming the URL is stored in the `Url` field
                        .FirstOrDefaultAsync();
            return certificate;

        }
    }
}
