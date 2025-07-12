using GamesList.Models;

namespace GamesList.Repositories.GenerosRepository
{
    public interface IGenerosRepository
    {
        public Task<List<Genero>> GetGenerosAsync();
        public Task<List<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids);
    }
}