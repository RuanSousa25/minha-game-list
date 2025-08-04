using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AuthService;
using GamesList.Services.JogoService;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.AvaliacaoService
{
    public class AvaliacaoService(IUnitOfWork uow, IJogoService jogoService, IAuthService authService, ILogger<AvaliacaoService> logger) : IAvaliacaoService
    {
        private readonly ILogger<AvaliacaoService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IJogoService _jogoService = jogoService;
        private readonly IAuthService _authService = authService;


        public Task<ServiceResultDto<Avaliacao>> GetAvaliacaoByIdAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResultDto<List<AvaliacaoResponseDto>>> GetAvaliacoesByJogoIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByJogoIdAsync(id);
            return Ok(avaliacoes.Select(AvaliacaoResponseDto.FromEntity).ToList());
        }

        public async Task<ServiceResultDto<List<AvaliacaoResponseDto>>> GetAvaliacoesByUsuarioIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByUsuarioIdAsync(id);
            return Ok(avaliacoes.Select(AvaliacaoResponseDto.FromEntity).ToList());
        }

        public async Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacoesByJogoIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByJogoIdAsync(id);
            if (avaliacoes == null || avaliacoes.Count == 0)
            {
                _logger.LogWarning("Nenhuma avaliação encontrada para o jogo de Id {id}", id);
                return NotFound<MessageResponseDto>("Nenhuma avaliação encontrada.");
            }
            _unitOfWork.AvaliacaoRepository.RemoveAvaliacoes(avaliacoes);
            _logger.LogInformation("Remoção de {length} avaliações do jogo de id {id} realizadas com sucesso.", avaliacoes!.Count, id);
            return Ok(new MessageResponseDto("Remoção de avaliações realizada com sucesso."));
        }
        public async Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacaoByIdAsync(int id, int userId, bool isAdmin)
        {
            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAvaliacaoByIdAsync(id);
            if (avaliacao == null)
            {
                _logger.LogWarning("Avaliação de id {id} não encontrada.", id);
                return NotFound<MessageResponseDto>("A avaliação não foi encontrada.");
            }
            if (avaliacao.Usuario.Id != userId && !isAdmin)
            {
                _logger.LogWarning("Usuário de id {id} não tem permissão o suficiente para remover a avaliação de id {id2}", id, userId);
                return Forbidden<MessageResponseDto>("Você não tem permissão para remover essa avaliação.");
            }

            _unitOfWork.AvaliacaoRepository.RemoveAvaliacao(avaliacao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("A avaliação de id {id} foi removida com sucesso.", id);
            return Ok(new MessageResponseDto("Avaliação removida com sucesso"));
        }

        public async Task<ServiceResultDto<AvaliacaoResponseDto>> SaveAvaliacaoAsync(int userId, AvaliacaoRequest request)
        {
            var resultDto = await _jogoService.GetJogoAsync(request.JogoId);
            if (!resultDto.Success) return new ServiceResultDto<AvaliacaoResponseDto> { StatusCode = resultDto.StatusCode, Message = resultDto.Message };
            var jogo = resultDto.Data;
            if (jogo == null)
            {
                _logger.LogWarning("Tentativa de avaliação para jogo inexistente. jogo: {id} | Usuario: {userId}.", request.JogoId, userId);
                return NotFound<AvaliacaoResponseDto>("Jogo não encontrado.");
            }
            if (request.Nota < 0 || request.Nota > 10)
            {
                _logger.LogWarning("Nota inválida (abaixo de 0 ou acima de 10). valor da nota: {nota}| jogo: {jogoId} | Usuario: {userId}.", request.Nota, request.JogoId, userId);
                return BadRequest<AvaliacaoResponseDto>("Nota inválida. Deve ser entre 0 e 10.");
            }

            var usuarioResult = await _authService.GetUsuarioByIdAsync(userId);
            if(!usuarioResult.Success){
                return NotFound<AvaliacaoResponseDto>("O usuário não foi encotnrado");
            }
            var usuario = usuarioResult.Data;

            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAvaliacaoByUsuarioIdAndJogoIdAsync(userId, request.JogoId);
            if (avaliacao == null)
            {
                avaliacao = new Avaliacao {
                    Usuario = usuario, Nota = request.Nota, Opiniao = request.Opiniao, DataCriacao = DateTime.UtcNow, Jogo = jogo };
                await _unitOfWork.AvaliacaoRepository.AddAvaliacaoAsync(avaliacao);
                _logger.LogInformation("Avaliação nova adicionada. Jogo: {jogoId} | Usuario: {userId}", request.JogoId, userId);
            }
            else
            {
                avaliacao.DataCriacao = DateTime.UtcNow;
                avaliacao.Opiniao = request.Opiniao;
                avaliacao.Nota = request.Nota;
                _unitOfWork.AvaliacaoRepository.UpdateAvaliacao(avaliacao);
                _logger.LogInformation("Avaliação atualizada. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
            }

            try
            {
                await _unitOfWork.CommitChangesAsync();
                _logger.LogInformation("Commit de avaliação realizado. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
                return Created(AvaliacaoResponseDto.FromEntity(avaliacao));
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no commit da avaliação. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId} | Ex: {ex}", avaliacao.Id, request.JogoId, userId, ex);
                return ServerError<AvaliacaoResponseDto>("Erro ao salvar avaliação.");
            }
        }

    }
}