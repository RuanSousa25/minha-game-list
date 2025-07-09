using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Repositories.JogoRepository;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.ImagensService;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.JogoService
{
    public class JogoService(IJogoRepository jogoRepository, IImagensService imagensService, IAvaliacaoService avaliacaoService, ILogger<JogoService> logger) : IJogoService
    {
        private readonly ILogger<JogoService> _logger = logger;
        private readonly IJogoRepository _repository = jogoRepository;
        private readonly IImagensService _imagensService = imagensService;
        private readonly IAvaliacaoService _avaliacaoService = avaliacaoService;
        public async Task<ServiceResultDto<List<JogoDTO>>> GetJogosList()
        {
            var jogos = await _repository.GetJogosAsync();
            return Ok(jogos.Select(j => new JogoDTO(j)).ToList());
        }
        public async Task<ServiceResultDto<string>> RemoveJogo(int id)
        {
            var jogo = await _repository.GetJogoComRelacionamentoByIdAsync(id);
            if (jogo == null)
            {
                _logger.LogWarning("Jogo {id} não econtrado no banco de dados.", id);
                return NotFound<string>("Jogo não encontrado.");
            }
            jogo.Generos.Clear();
            await _avaliacaoService.RemoveAvaliacoesByJogoId(id);
            await _imagensService.RemoveImagensByJogoId(id);
            try
            {
                await _repository.SaveChangesAsync();
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