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
        public async Task<ServiceResultDto<string>> RemoveJogo(int id)
        {
            var jogo = await _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .FirstOrDefaultAsync(j => j.Id == id);
            if (jogo == null) return NotFound<string>("Jogo não encontrado.");
            jogo.Generos.Clear();
            _appDbContext.Avaliacoes.RemoveRange(jogo.Avaliacoes);
            _appDbContext.Imagens.RemoveRange(jogo.Imagens);
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return ServerError<string>("Não foi possível realizar a exclusão. Error: "+e);
            }
            return Ok("Exclusão realizada com sucesso");
        }
    }
}