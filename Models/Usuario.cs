namespace GamesList.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string SenhaHash { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; } = [];
    }
}