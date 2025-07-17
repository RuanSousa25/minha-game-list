namespace GamesList.Dtos.Responses
{
    public class MessageResponseDto(string message)
    {
        public string Message { get; set; } = message;
    }
}