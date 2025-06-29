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
        public DbSet<ImagemSugestao> ImagemSugestao => Set<ImagemSugestao>();
        
        
        
    }
}