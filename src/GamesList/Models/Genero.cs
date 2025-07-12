using System.Text.Json.Serialization;

namespace GamesList.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        [JsonIgnore]
        public ICollection<Jogo> Jogos { get; set; } = [];
        [JsonIgnore]
        public ICollection<SugerirJogo> JogosSugeridos { get; set; } = [];
    }
}