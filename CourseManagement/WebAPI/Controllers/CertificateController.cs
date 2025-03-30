using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ICertificateService _certificateService;

        public CertificateController(IFileService fileService,ICertificateService certificateService)
        {
            _fileService = fileService;
            _certificateService = certificateService;
        }

        [HttpPost("/upload/template")]
        public async Task<ActionResult<CertificateTemplateRes>> UploadCertificateTeamplate(IFormFile cerPdf)
        {
            CertificateTemplateRes certificateUrl = await _certificateService.UploadCertificateUrl(cerPdf);
            return certificateUrl;
        }

        [HttpGet("/template")]
        public async Task<ActionResult<string>> GetCertificateUrlTemplate()
        {
            var certificateUrl = await _certificateService.GetCertificateTemplateUrl();
            return Ok(certificateUrl);
        }


        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate(CertificateRequest request)
        {
            var categories = await _fileService.GenerateAndUploadCertificateAsync(request);
            return Ok(categories);
        }
        [HttpGet]
        public async Task<ActionResult<CertificateUserRes>> GetCertificateUrl(int enrollmentId)
        {
            var certificateUrl = await _certificateService.GetCertificate(enrollmentId);
            return Ok(certificateUrl);
        }
    }
}
