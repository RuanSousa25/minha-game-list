using GamesList.DTOs;
using GamesList.Models;

namespace GamesList.Services.JogoService
{
    public interface IJogoService
    {
        public Task<ServiceResultDto<List<JogoDTO>>> GetJogosList();
        public Task<ServiceResultDto<string>> RemoveJogo(int id);
        public Task<ServiceResultDto<Jogo>> AddJogoAsync(Jogo jogo);
        public Task<ServiceResultDto<bool>> CheckIfJogoExistsAsync(int id);
    }
}