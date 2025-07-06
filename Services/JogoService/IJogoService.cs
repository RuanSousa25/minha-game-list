using GamesList.DTOs;

namespace GamesList.Services.JogoService
{
    public interface IJogoService
    {
        public Task<ServiceResultDto<List<JogoDTO>>> GetJogosList();
        public Task<ServiceResultDto<string>> RemoveJogo(int id);
    }
}