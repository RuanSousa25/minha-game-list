using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.ImagensService;
using static GamesList.Dtos.Helpers.Results;

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
        public async Task<ServiceResultDto<PagedResult<JogoDto>>> GetJogosPagedAsync(PaginationParams paginationParams)
        {
            return Ok(await _unitOfWork.JogoRepository.GetJogosPagedAsync(paginationParams));
        }
        public async Task<ServiceResultDto<MessageResponseDto>> RemoveJogoAsync(int id)
        {
            var jogo = await _unitOfWork.JogoRepository.GetJogoComRelacionamentoByIdAsync(id);
            if (jogo == null)
            {
                _logger.LogWarning("Jogo {id} não econtrado no banco de dados.", id);
                return NotFound<MessageResponseDto>("Jogo não encontrado.");
            }
            await _unitOfWork.JogoRepository.RemoveJogoByIdAsync(id);
            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível remover o jogo {nome}. JogoId: {id}", jogo.Nome, jogo.Id);
                return ServerError<MessageResponseDto>("Não foi possível realizar a exclusão. Error: " + e);
            }
            _logger.LogInformation("Jogo {nome} foi removido com sucesso. JogoId: {id}", jogo.Nome, jogo.Id);
            return Ok(new MessageResponseDto("Exclusão realizada com sucesso"));
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

        public async Task<ServiceResultDto<JogoDto>> GetJogoDtoAsync(int id)
        {
            var jogo = await _unitOfWork.JogoRepository.GetJogoAsync(id);
            if (jogo == null)
            {
                return NotFound<JogoDto>("Jogo não encontrado");
            }

            return Ok(new JogoDto(jogo));
        }   
         public async Task<ServiceResultDto<Jogo>> GetJogoAsync(int id)
        {
            var jogo = await _unitOfWork.JogoRepository.GetJogoAsync(id);
            if (jogo == null)
            {
                return NotFound<Jogo>("Jogo não encontrado");
            }

            return Ok(jogo);
        }


    }
}