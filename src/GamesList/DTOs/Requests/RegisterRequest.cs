namespace GamesList.Dtos.Requests
{
    public class RegisterRequest
    {
        public required string Login { get; set; }
        public required string Senha { get; set; }
    }
}