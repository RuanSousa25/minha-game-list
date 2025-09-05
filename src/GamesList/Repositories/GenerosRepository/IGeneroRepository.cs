using GamesList.Common.Pagination;
using GamesList.Models;

namespace GamesList.Repositories.GeneroRepository
{
    public interface IGeneroRepository
    {
        public Task<PagedResult<Genero>> GetGenerosAsync(PaginationParams paginationParams);
        public Task<PagedResult<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids, PaginationParams paginationParams);
    }
}