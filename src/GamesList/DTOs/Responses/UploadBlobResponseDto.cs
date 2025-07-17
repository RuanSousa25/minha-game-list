namespace GamesList.Dtos.Responses
{
    public class UploadBlobResponseDto(string url)
    {
        public string Url { get; set; } = url;
    }
}