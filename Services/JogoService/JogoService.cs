using GamesList.Databases;
using GamesList.DTOs;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.JogoService
{
    public class JogoService(AppDbContext appDbContext, ILogger<JogoService> logger) : IJogoService
    {
        private readonly ILogger<JogoService> _logger = logger;
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
            if (jogo == null)
            {
                _logger.LogWarning("Jogo {id} não econtrado no banco de dados.", id);
                return NotFound<string>("Jogo não encontrado.");
            }
            jogo.Generos.Clear();
            _appDbContext.Avaliacoes.RemoveRange(jogo.Avaliacoes);
            _appDbContext.Imagens.RemoveRange(jogo.Imagens);
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível remover o jogo {nome}. JogoId: {id}", jogo.Nome, jogo.Id);
                return ServerError<string>("Não foi possível realizar a exclusão. Error: " + e);
            }
            _logger.LogInformation("Jogo {nome} foi removido com sucesso. JogoId: {id}", jogo.Nome, jogo.Id);
            return Ok("Exclusão realizada com sucesso");
        }
    }
}