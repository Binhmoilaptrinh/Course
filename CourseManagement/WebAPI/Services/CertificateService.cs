using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;


namespace WebAPI.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ECourseContext _eCourseContext;
        private readonly Cloudinary _cloudinary;

        public CertificateService(ECourseContext eCourseContext, IFileService fileService)
        {
            _eCourseContext = eCourseContext;
            var account = new Account("doslvje9p", "111261246971633", "lMdfdvz3SsDnpHJA_WDBtRQmFKU");
            _cloudinary = new Cloudinary(account);
        }

        public async Task<CertificateTemplateRes> GetCertificateTemplateUrl()
        {
            var existingTemplate = await _eCourseContext.CertificateTemplate.FirstOrDefaultAsync();
            var template = new CertificateTemplateRes
            {
                templateUrl = ""
            };
            if (existingTemplate != null)
            {
                template.templateUrl = existingTemplate.Url;
            }
            return template;
        }

        public async Task<CertificateUserRes> GetCertificate(int enrollmentId)
        {
            var enrollment = await _eCourseContext.Enrollments
                .Include(u => u.User)
                .Include(c => c.Course)
                .Select(x => new 
                {
                    id = x.Id,
                    userName = x.User.Username,
                    courseName = x.Course.Title,
                    progress = x.Progress
                })
                .FirstAsync(x => x.id == enrollmentId);

            if(enrollment != null && enrollment.progress == 100.0)
            {
                var existingTemplate = await _eCourseContext.CertificateTemplate.FirstOrDefaultAsync();
                if(existingTemplate != null && !existingTemplate.Url.IsNullOrEmpty())
                {
                    return new CertificateUserRes
                    {
                        userName = enrollment.userName,
                        courseName = enrollment.courseName,
                        certificate = existingTemplate.Url.ToString()
                    };
                }
            }
            return null;
        }

        public async Task<CertificateTemplateRes> UploadCertificateUrl(IFormFile file)
        {
            var certificateUrl = "";
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                certificateUrl = uploadResult.SecureUrl.ToString();
            }

            var existingTemplate = await _eCourseContext.CertificateTemplate.FirstOrDefaultAsync();

            if (existingTemplate != null)
            {
                existingTemplate.Url = certificateUrl;
            }
            else
            {
                var newTemplate = new CertificateTemplate
                {
                    Url = certificateUrl
                };
                _eCourseContext.CertificateTemplate.Add(newTemplate);
            }
            await _eCourseContext.SaveChangesAsync();
            return new CertificateTemplateRes
            {
                templateUrl = certificateUrl
            };
        }
    }
}
