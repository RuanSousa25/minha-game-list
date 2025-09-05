using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Models;

namespace GamesList.Services.GeneroService
{
    public interface IGeneroService
    {
        public Task<ServiceResultDto<PagedResult<Genero>>> ListGenerosAsync(PaginationParams paginationParams);
        public Task<ServiceResultDto<PagedResult<Genero>>> ListGenerosByIdsAsync(List<int> ids, PaginationParams paginationParams);
    }
}