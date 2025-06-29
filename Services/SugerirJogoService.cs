using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;

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
    }
}