using GamesList.DTOs;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.ImagensService;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.JogoService
{
    public class JogoService(IUnitOfWork uow,
    IImagensService imagensService,
    Lazy<IAvaliacaoService> avaliacaoService,
    ILogger<JogoService> logger) : IJogoService
    {
        private readonly ILogger<JogoService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IImagensService _imagensService = imagensService;
        private readonly Lazy<IAvaliacaoService> _avaliacaoService = avaliacaoService;
        public async Task<ServiceResultDto<List<JogoDTO>>> ListJogosAsync()
        {
            var jogos = await _unitOfWork.JogoRepository.GetJogosAsync();
            return Ok(jogos.Select(j => new JogoDTO(j)).ToList());
        }
        public async Task<ServiceResultDto<string>> RemoveJogoAsync(int id)
        {
            var jogo = await _unitOfWork.JogoRepository.GetJogoComRelacionamentoByIdAsync(id);
            if (jogo == null)
            {
                _logger.LogWarning("Jogo {id} não econtrado no banco de dados.", id);
                return NotFound<string>("Jogo não encontrado.");
            }
            jogo.Generos.Clear();
            await _avaliacaoService.Value.RemoveAvaliacoesByJogoIdAsync(id);
            await _imagensService.RemoveImagensByJogoIdAsync(id);
            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível remover o jogo {nome}. JogoId: {id}", jogo.Nome, jogo.Id);
                return ServerError<string>("Não foi possível realizar a exclusão. Error: " + e);
            }
            _logger.LogInformation("Jogo {nome} foi removido com sucesso. JogoId: {id}", jogo.Nome, jogo.Id);
            return Ok("Exclusão realizada com sucesso");
        }
        public async Task<ServiceResultDto<Jogo>> AddJogoAsync(Jogo jogo)
        {
            await _unitOfWork.JogoRepository.AddJogoAsync(jogo);
            return Ok(jogo);
        }
        public async Task<ServiceResultDto<bool>> CheckIfJogoExistsAsync(int id)
        {
            var result = await _unitOfWork.JogoRepository.CheckIfJogoExistsAsync(id);
            return Ok(result);
        }

        public async Task<ServiceResultDto<JogoDTO>> GetJogoAsync(int id)
        {
            var jogo = await _unitOfWork.JogoRepository.GetJogoAsync(id);
            if (jogo == null)
            {
                return NotFound<JogoDTO>("Jogo não encontrado");
            }

            return Ok(new JogoDTO(jogo));
        }   
    }
}