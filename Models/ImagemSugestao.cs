namespace GamesList.Models
{
    public class ImagemSugestao
    {
        public int Id { get; set; }
        public int SugerirJogoId { get; set; }
        public int TipoId { get; set; }
        public required string Url { get; set; }
    }
}