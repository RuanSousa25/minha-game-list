namespace GamesList.Models
{
    public class SugestaoImagem
    {
        public int Id { get; set; }
        public int SugestaoJogoId { get; set; }
        public int TipoId { get; set; }
        public required string Url { get; set; }
    }
}