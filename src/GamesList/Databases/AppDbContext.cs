using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Databases
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
        public DbSet<Jogo> Jogos => Set<Jogo>();
        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Imagem> Imagens => Set<Imagem>();
        public DbSet<TipoImagem> TiposImagem => Set<TipoImagem>();
        public DbSet<SugestaoJogo> SugestoesJogo => Set<SugestaoJogo>();
        public DbSet<SugestaoImagem> SugestoesImagem => Set<SugestaoImagem>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("minha_game_list");

            modelBuilder.Entity<Jogo>()
                .HasMany(j => j.Generos)
                .WithMany(g => g.Jogos)
                .UsingEntity<Dictionary<string, object>>(
                    "JogosGeneros",
                    j => j.HasOne<Genero>().WithMany().HasForeignKey("GeneroId"),
                    g => g.HasOne<Jogo>().WithMany().HasForeignKey("JogoId")
                );
            modelBuilder.Entity<SugestaoJogo>()
                .HasMany(j => j.Generos)
                .WithMany(g => g.JogosSugeridos)
                .UsingEntity<Dictionary<string, object>>(
                    "SugestoesJogoGeneros",
                    j => j.HasOne<Genero>().WithMany().HasForeignKey("GeneroId"),
                    g => g.HasOne<SugestaoJogo>().WithMany().HasForeignKey("SugestaoJogoId")
                );

        }

    }
}