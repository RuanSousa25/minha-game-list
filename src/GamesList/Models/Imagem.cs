namespace GamesList.Models
{
    public class Imagem
    {
        public int Id { get; set; }
        public int JogoId { get; set; }
        public int TipoId{ get; set; }
        public required string Url { get; set; }
    }
}