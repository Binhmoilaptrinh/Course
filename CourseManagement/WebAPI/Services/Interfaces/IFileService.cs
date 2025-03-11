using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<BlobDto>> ListAsync();
        Task<BlobResponseDto> UploadAsync(IFormFile blob);
    }
}
