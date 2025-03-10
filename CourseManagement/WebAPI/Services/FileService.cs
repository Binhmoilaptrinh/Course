using Azure.Storage;
using Azure.Storage.Blobs;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class FileService : IFileService
    {
        private readonly string _storageAccount = "elearningcourse";
        private readonly string _key = "d7y4wTEjIl0mMyy9Tlu6Ds9qgS0twnsKblHJ+aexF6PPg/0IoCk+cpT3QSMBazoU/0C+e4krt9cr+AStz43GaA==";
        private BlobContainerClient _filesContainer;
        private readonly ECourseContext _context;

        public FileService(ECourseContext context)
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("web");
            _context = context;
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
    }
}