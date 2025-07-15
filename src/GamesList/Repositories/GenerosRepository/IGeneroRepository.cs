using GamesList.Models;

namespace GamesList.Repositories.GeneroRepository
{
    public interface IGeneroRepository
    {
        public Task<List<Genero>> GetGenerosAsync();
        public Task<List<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids);
    }
}