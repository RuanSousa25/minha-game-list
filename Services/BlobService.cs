using Azure.Storage.Blobs;
using GamesList.DTOs;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services
{
    public class BlobService
    {

        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)
        {
            var connString = config.GetSection("AZURE_BLOB_CONNECTION_STRING").Value;
            var containerName = config.GetSection("CONTAINER_NAME").Value;
            _containerClient = new BlobContainerClient(connString, containerName);
        }
        public async Task<ServiceResultDto<string>> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
            var url = blobClient.Uri.ToString();
            if (string.IsNullOrWhiteSpace(url)) return ServerError<string>("Ocorreu um erro ao fazer o upload da imagem");
            return ServiceResultDto<string>.Ok(url);
        }
    }
}