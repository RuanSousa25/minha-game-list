using GamesList.Dtos;
using GamesList.Models;

namespace GamesList.Services.GeneroService
{
    public interface IGeneroService
    {
        public Task<ServiceResultDto<List<Genero>>> ListGenerosAsync(string? search);
        public Task<ServiceResultDto<List<Genero>>> ListGenerosByIdsAsync(List<int> ids);
    }
}