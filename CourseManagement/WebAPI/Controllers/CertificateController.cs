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

        public CertificateController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate(CertificateRequest request)
        {
            var categories = await _fileService.GenerateAndUploadCertificateAsync(request);
            return Ok(categories);
        }
    }
}
