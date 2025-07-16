using GamesList.DTOs;
using GamesList.Models;

namespace GamesList.Services.JogoService
{
    public interface IJogoService
    {
        public Task<ServiceResultDto<List<JogoDTO>>> ListJogosAsync();
        public Task<ServiceResultDto<string>> RemoveJogoAsync(int id);
        public Task<ServiceResultDto<Jogo>> AddJogoAsync(Jogo jogo);
        public Task<ServiceResultDto<bool>> CheckIfJogoExistsAsync(int id);
        public Task<ServiceResultDto<JogoDTO>> GetJogoAsync(int id);
    }
}