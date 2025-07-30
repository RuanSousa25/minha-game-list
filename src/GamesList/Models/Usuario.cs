using System.Text.Json.Serialization;

namespace GamesList.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        [JsonIgnore]
        public required string SenhaHash { get; set; }
        [JsonIgnore]
        public required Role Role { get; set; }
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacoes { get; set; } = [];
    }
}