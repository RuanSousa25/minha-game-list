using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Services
{
    public class SugerirJogoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<int>> SaveSugestaoJogo(UploadGameRequest request, int userId)
        {
            var generos = _appDbContext.Generos.Where(g => request.Generos.Contains(g.Id)).ToList();
            var sugestao = new SugerirJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos, DataSugestao = DateTime.UtcNow, Aprovado = false };
            _appDbContext.SugerirJogo.Add(sugestao);
            await _appDbContext.SaveChangesAsync();
            return ServiceResultDto<int>.Ok(sugestao.Id);
        }

        public async Task<ServiceResultDto<JogoDTO>> AprovarJogo(int id)
        {
            var sugestao =
            await _appDbContext.SugerirJogo
            .Include(s => s.Generos)
            .Include(s => s.Imagens)
            .FirstOrDefaultAsync(s => s.Id == id);
            
            if (sugestao == null) return ServiceResultDto<JogoDTO>.Fail("Sugestão não encontrada.");
            sugestao.Aprovado = true;

            var generos = sugestao.Generos.Select(g => _appDbContext.Generos.Find(g.Id)!).ToList();
            var jogo = new Jogo { Generos = generos, Nome = sugestao.Nome };
            await _appDbContext.Jogos.AddAsync(jogo);
            await _appDbContext.SaveChangesAsync();

            var imagens = sugestao.Imagens;
            foreach (var imagem in imagens)
            {
                await _appDbContext.Imagens.AddAsync(new Imagem { Url = imagem.Url, JogoId = jogo.Id, TipoId = imagem.TipoId});
            }
            await _appDbContext.SaveChangesAsync();
            return ServiceResultDto<JogoDTO>.Ok(new JogoDTO(jogo));
        }
    }
}