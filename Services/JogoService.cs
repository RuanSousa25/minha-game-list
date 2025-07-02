using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;
namespace GamesList.Services
{
    public class JogoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<List<JogoDTO>>> GetJogosList()
        {
            var jogos = await _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .Select(j => new JogoDTO(j))
            .ToListAsync();

            return Ok(jogos);
        }
    }
}