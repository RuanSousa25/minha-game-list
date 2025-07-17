namespace GamesList.Dtos.Responses
{
    public class LoginResponseDto(string token)
    {
        public string AccessToken { get; set; } = token;
    }
}