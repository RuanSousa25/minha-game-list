
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.BlobService
{
    public class GCBlobService : IBlobService
    {
        private readonly ILogger<GCBlobService> _logger;


        public GCBlobService(IConfiguration config, ILogger<GCBlobService> logger)
        {
            var connString = config.GetSection("GC_BLOB_CONNECTION_STRING").Value;
            var containerName = config.GetSection("CONTAINER_NAME").Value;
            _logger = logger;
        }

        public Task<ServiceResultDto<MessageResponseDto>> DeleteFileAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResultDto<UploadBlobResponseDto>> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
             throw new NotImplementedException();
        }

    }
}
