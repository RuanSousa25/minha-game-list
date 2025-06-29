using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Services
{
    public class JogoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public  ICollection<JogoDTO> GetJogosList()
        {
            var jogos = _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .Select(j => new JogoDTO(j))
            .ToList();

            return jogos;
        }
    }
}