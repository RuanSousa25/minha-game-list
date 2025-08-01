
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.BlobService
{
    public class GCBlobService : IBlobService
    {
        private readonly ILogger<GCBlobService> _logger;
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;


        public GCBlobService(IConfiguration config, ILogger<GCBlobService> logger)
        {
            _logger = logger;
            _bucketName = config["GCS_BUCKET_NAME"] ?? throw new ArgumentNullException("GCS_BUCKET_NAME Inexistente");
            var credentialsPath = config["GCS_CREDENTIALS_PATH"] ?? throw new ArgumentNullException("GCS_CREDENTIALS_PATH Inexistente");
            var credential = GoogleCredential.FromFile(credentialsPath);
            _storageClient = StorageClient.Create(credential);

        }

        public async Task<ServiceResultDto<MessageResponseDto>> DeleteFileAsync(string url)
        {
            var fileName = Path.GetFileName(new Uri(url).LocalPath);

            await _storageClient.DeleteObjectAsync(_bucketName, fileName);
            _logger.LogInformation("Arquivo {filename} deletado com sucesso do bucket {bucketname}", fileName, _bucketName);
            return Ok(new MessageResponseDto("Imagem Deletada com sucesso"));
        }

        public async Task<ServiceResultDto<UploadBlobResponseDto>> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var obj = await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, fileStream);

            _logger.LogInformation("Enviando arquivo {fileName} para bucket {bucketName}", fileName, _bucketName);
            return Created(new UploadBlobResponseDto($"https://storage.googleapis.com/{_bucketName}/{Uri.EscapeDataString(obj.Name)}"));

        }

    }
}
