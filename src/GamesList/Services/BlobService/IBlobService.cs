using GamesList.Dtos;
using GamesList.Dtos.Responses;

namespace GamesList.Services.BlobService
{
    public interface IBlobService
    {
        public Task<ServiceResultDto<MessageResponseDto>> DeleteFileAsync(string url);

        public Task<ServiceResultDto<UploadBlobResponseDto>> UploadFileAsync(Stream filestream, string fileName, string contentType);
    }   
}