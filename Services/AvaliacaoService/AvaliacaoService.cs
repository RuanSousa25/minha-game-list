using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.AvaliacaoService
{
    public class AvaliacaoService(AppDbContext appDbContext, ILogger<AvaliacaoService> logger) : IAvaliacaoService
    {
        private readonly ILogger<AvaliacaoService> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoId(int id)
        {
            var avaliacoes = await _appDbContext.Avaliacoes.Where(a => a.JogoId == id).ToListAsync();
            return Ok(avaliacoes);
        }

        public async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioId(int id)
        {
            var avaliacoes = await _appDbContext.Avaliacoes.Where(a => a.UsuarioId == id).ToListAsync();
            return Ok(avaliacoes);
        }

        public async Task<ServiceResultDto<string>> RemoveAvaliacoesByJogoId(int id)
        {
            var getResult = await GetAvaliacoesByJogoId(id);
            if (getResult == null) {
                _logger.LogError("Não foi possível remover as avaliações do jogo de Id {id}", id);
                return ServerError<string>("Não foi possível remover as avaliações");
            }
            var avaliacoes = getResult.Data;
            
            _appDbContext.Avaliacoes.RemoveRange(avaliacoes!);
            _logger.LogInformation("Remoção de {length} avaliações do jogo de id {id} realizadas com sucesso.", avaliacoes!.Count, id);
            return Ok("Remoção de avaliações realizada com sucesso.");
        }

        public async Task<ServiceResultDto<string>> SaveAvaliacao(int userId, AvaliacaoRequest request)
        {
            var jogoExiste = await _appDbContext.Jogos.AnyAsync(j => j.Id == request.JogoId);

            if (!jogoExiste)
            {
                _logger.LogWarning("Tentativa de avaliação para jogo inexistente. jogo: {id} | Usuario: {userId}.", request.JogoId, userId);
                return NotFound<string>("Jogo não encontrado.");
            }
            if (request.Nota < 0 || request.Nota > 10)
            {
                _logger.LogWarning("Nota inválida (abaixo de 0 ou acima de 10). valor da nota: {nota}| jogo: {jogoId} | Usuario: {userId}.", request.Nota,request.JogoId, userId);
                return BadRequest<string>("Nota inválida. Deve ser entre 0 e 10.");
            }

            var avaliacao =
            await _appDbContext.Avaliacoes
            .FirstOrDefaultAsync(a => a.UsuarioId == userId && a.JogoId == request.JogoId);

            if (avaliacao == null)
            {
                avaliacao = new Avaliacao { UsuarioId = userId, Nota = request.Nota, Opiniao = request.Opiniao, Data = DateTime.UtcNow, JogoId = request.JogoId };
                await _appDbContext.Avaliacoes.AddAsync(avaliacao);
                _logger.LogInformation("Avaliação nova adicionada. Jogo: {jogoId} | Usuario: {userId}", request.JogoId, userId);
            }
            else
            {
                avaliacao.Data = DateTime.UtcNow;
                avaliacao.Opiniao = request.Opiniao;
                avaliacao.Nota = request.Nota;
                _appDbContext.Update(avaliacao);
                _logger.LogInformation("Avaliação atualizada. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
            }

            try {
                await _appDbContext.SaveChangesAsync();
                 _logger.LogInformation("Commit de avaliação realizado. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId}", avaliacao.Id, request.JogoId, userId);
                return Ok("Avaliação postada com sucesso.");
            } catch (Exception ex) {
                 _logger.LogError("Erro no commit da avaliação. Avaliacao: {avId} | Jogo: {jogoId} | Usuario: {userId} | Ex: {ex}", avaliacao.Id, request.JogoId, userId, ex);
                return ServerError<string>("Erro ao salvar avaliação.");
            }
        }
    }
}