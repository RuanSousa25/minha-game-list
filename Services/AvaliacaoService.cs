using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services
{
    public class AvaliacaoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByJogoId(int id)
        {
            var avaliacoes = await _appDbContext.Avaliacoes.Where(a => a.JogoId == id).ToListAsync();
            return Ok(avaliacoes);
        }

        internal async Task<ServiceResultDto<List<Avaliacao>>> GetAvaliacoesByUsuarioId(int id)
        {
            var avaliacoes = await _appDbContext.Avaliacoes.Where(a => a.UsuarioId == id).ToListAsync();
            return Ok(avaliacoes);
        }

        internal async Task<ServiceResultDto<string>> SaveAvaliacao(int userId, AvaliacaoRequest request)
        {
            var jogoExiste = await _appDbContext.Jogos.AnyAsync(j => j.Id == request.JogoId);

            if (!jogoExiste) return NotFound<string>("Jogo não encontrado.");
            if (request.Nota < 0 || request.Nota > 10) return BadRequest<string>("Nota inválida. Deve ser entre 1 e 10.");

            var avaliacao =
            await _appDbContext.Avaliacoes
            .FirstOrDefaultAsync(a => a.UsuarioId == userId && a.JogoId == request.JogoId);

            if (avaliacao == null)
            {
                avaliacao = new Avaliacao { UsuarioId = userId, Nota = request.Nota, Opiniao = request.Opiniao, Data = DateTime.UtcNow, JogoId = request.JogoId };
                _appDbContext.Avaliacoes.Add(avaliacao);
            }
            else
            {
                avaliacao.Data = DateTime.UtcNow;
                avaliacao.Opiniao = request.Opiniao;
                avaliacao.Nota = request.Nota;
                _appDbContext.Update(avaliacao);
            }

            try {
                await _appDbContext.SaveChangesAsync();
                return Ok("Avaliação postada com sucesso.");
            } catch (Exception ex) {
                return ServerError<string>("Erro ao salvar avaliação. Error: "+ex);
            }
        }
    }
}