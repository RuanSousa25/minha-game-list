using GamesList.Common.Pagination;
using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.GeneroRepository
{
    public class GeneroRepository(AppDbContext appDbContext) : IGeneroRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<PagedResult<Genero>> GetGenerosAsync(PaginationParams paginationParams)
        {
            return await _appDbContext.Generos
            .Where(g => paginationParams.Search == null
            || g.Nome.ToLower().Contains(paginationParams.Search.ToLower()))
            .ToPagedResultAsync(paginationParams);
        }

        public async Task<PagedResult<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids, PaginationParams paginationParams)
        {
            return await _appDbContext.Generos
            .Where(g => ids.Contains(g.Id))
            .Where(g => paginationParams.Search == null
            || g.Nome.ToLower().Contains(paginationParams.Search.ToLower()))
            .ToPagedResultAsync(paginationParams);
        }
    }
}