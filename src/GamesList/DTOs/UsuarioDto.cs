using System.Text.Json.Serialization;
using GamesList.Models;

namespace GamesList.Dtos
{
    public class UsuarioDto(Usuario usuario)
    {
        public int Id { get; set; } = usuario.Id;
        public string Login { get; set; } = usuario.Login;
        public string Role { get; set; } = usuario.Role.Nome;
    }
}