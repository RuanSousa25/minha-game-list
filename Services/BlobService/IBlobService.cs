using GamesList.DTOs;

namespace GamesList.Services.BlobService
{
    public interface IBlobService
    {
        public Task<ServiceResultDto<string>> UploadFileAsync(Stream filestream, string fileName, string contentType);
    }   
}