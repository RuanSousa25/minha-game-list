using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Databases
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
        public DbSet<Jogo> Jogos => Set<Jogo>();
        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Imagem> Imagens => Set<Imagem>();
        public DbSet<TipoImagem> TiposImagens => Set<TipoImagem>();
        public DbSet<SugerirJogo> SugerirJogo => Set<SugerirJogo>();
        public DbSet<ImagensSugestao> ImagensSugestao => Set<ImagensSugestao>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jogo>()
                .HasMany(j => j.Generos)
                .WithMany(g => g.Jogos)
                .UsingEntity<Dictionary<string, object>>(
                    "JogosGeneros",
                    j => j.HasOne<Genero>().WithMany().HasForeignKey("GeneroId"),
                    g => g.HasOne<Jogo>().WithMany().HasForeignKey("JogoId")
                );
            modelBuilder.Entity<SugerirJogo>()
                .HasMany(j => j.Generos)
                .WithMany(g => g.JogosSugeridos)
                .UsingEntity<Dictionary<string, object>>(
                    "SugerirJogoGeneros",
                    j => j.HasOne<Genero>().WithMany().HasForeignKey("GeneroId"),
                    g => g.HasOne<SugerirJogo>().WithMany().HasForeignKey("SugerirJogoId")
                );
            
        }
    }
}