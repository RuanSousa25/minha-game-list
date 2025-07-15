using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.JogoService;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.AvaliacaoService
{
    public class AvaliacaoService(IUnitOfWork uow, IJogoService jogoService, ILogger<AvaliacaoService> logger) : IAvaliacaoService
    {
        private readonly ILogger<AvaliacaoService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IJogoService _jogoService = jogoService;


        public Task<ServiceResultDto<Avaliacao>> GetAvaliacaoByIdAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByJogoIdAsync(id);
            return Ok(avaliacoes);
        }

        public async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByUsuarioIdAsync(id);
            return Ok(avaliacoes);
        }

        public async Task<ServiceResultDto<string>> RemoveAvaliacoesByJogoIdAsync(int id)
        {
            var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAvaliacoesByJogoIdAsync(id);
            if (avaliacoes == null || avaliacoes.Count == 0)
            {
                _logger.LogWarning("Nenhuma avaliação encontrada para o jogo de Id {id}", id);
                return NotFound<string>("Nenhuma avaliação encontrada.");
            }
            _unitOfWork.AvaliacaoRepository.RemoveAvaliacoes(avaliacoes);
            _logger.LogInformation("Remoção de {length} avaliações do jogo de id {id} realizadas com sucesso.", avaliacoes!.Count, id);
            return Ok("Remoção de avaliações realizada com sucesso.");
        }
        public async Task<ServiceResultDto<string>> RemoveAvaliacaoByIdAsync(int id, int userId, bool isAdmin)
        {
            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAvaliacaoByIdAsync(id);
            if (avaliacao == null)
            {
                _logger.LogWarning("Avaliação de id {id} não encontrada.", id);
                return NotFound<string>("A avaliação não foi encontrada.");
            }
            if (avaliacao.UsuarioId != userId && !isAdmin)
            {
                _logger.LogWarning("Usuário de id {id} não tem permissão o suficiente para remover a avaliação de id {id2}", id, userId);
                return Forbidden<string>("Você não tem permissão para remover essa avaliação.");
            }

            _unitOfWork.AvaliacaoRepository.RemoveAvaliacao(avaliacao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("A avaliação de id {id} foi removida com sucesso.", id);
            return Ok("Avaliação removida com sucesso.");
        }

        public async Task<ServiceResultDto<string>> SaveAvaliacaoAsync(int userId, AvaliacaoRequest request)
        {
            var resultDto = await _jogoService.CheckIfJogoExistsAsync(request.JogoId);
            if (!resultDto.Success) return new ServiceResultDto<string> { StatusCode = resultDto.StatusCode, Message = resultDto.Message };
            var jogoExiste = resultDto.Data;
            if (!jogoExiste)
            {
                _logger.LogWarning("Tentativa de avaliação para jogo inexistente. jogo: {id} | Usuario: {userId}.", request.JogoId, userId);
                return NotFound<string>("Jogo não encontrado.");
            }
            if (request.Nota < 0 || request.Nota > 10)
            {
                _logger.LogWarning("Nota inválida (abaixo de 0 ou acima de 10). valor da nota: {nota}| jogo: {jogoId} | Usuario: {userId}.", request.Nota, request.JogoId, userId);
                return BadRequest<string>("Nota inválida. Deve ser entre 0 e 10.");
            }

            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAvaliacaoByUsuarioIdAndJogoIdAsync(userId, request.JogoId);

            if (avaliacao == null)
            {
                avaliacao = new Avaliacao { UsuarioId = userId, Nota = request.Nota, Opiniao = request.Opiniao, Data = DateTime.UtcNow, JogoId = request.JogoId };
                await _unitOfWork.AvaliacaoRepository.AddAvaliacaoAsync(avaliacao);
                _logger.LogInformation("Avaliação nova adicionada. Jogo: {jogoId} | Usuario: {userId}", request.JogoId, userId);
            }
            else
            {
                avaliacao.Data = DateTime.UtcNow;
                avaliacao.Opiniao = request.Opiniao;
                avaliacao.Nota = request.Nota;
                _unitOfWork.AvaliacaoRepository.UpdateAvaliacao(avaliacao);
                _logger.LogInformation("Avaliação atualizada. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
            }

            try
            {
                await _unitOfWork.CommitChangesAsync();
                _logger.LogInformation("Commit de avaliação realizado. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
                return Ok("Avaliação postada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no commit da avaliação. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId} | Ex: {ex}", avaliacao.Id, request.JogoId, userId, ex);
                return ServerError<string>("Erro ao salvar avaliação.");
            }
        }

    }
}