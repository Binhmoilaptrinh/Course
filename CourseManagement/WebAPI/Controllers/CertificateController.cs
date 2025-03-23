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

        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate(CertificateRequest request)
        {
            var categories = await _fileService.GenerateAndUploadCertificateAsync(request);
            return Ok(categories);
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetCertificateUrl(int enrollmentId)
        {
            var certificateUrl = await _certificateService.GetCertificateUrl(enrollmentId);
            return Ok(certificateUrl);
        }
    }
}
