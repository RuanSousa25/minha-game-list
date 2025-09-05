using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using static GamesList.Dtos.Helpers.Results;
namespace GamesList.Services.GeneroService
{
    public class GeneroService(IUnitOfWork uow, ILogger<GeneroService> logger) : IGeneroService
    {
        private ILogger<GeneroService> _logger = logger;
        private IUnitOfWork _unitOfWork = uow;

        public async Task<ServiceResultDto<PagedResult<Genero>>> ListGenerosAsync(PaginationParams paginationParams)
        {
            return Ok(await _unitOfWork.GeneroRepository.GetGenerosAsync(paginationParams));
        }

        public async Task<ServiceResultDto<PagedResult<Genero>>> ListGenerosByIdsAsync(List<int> ids, PaginationParams paginationParams)
        {
            return Ok(await _unitOfWork.GeneroRepository.GetGenerosByGenerosIdsAsync(ids, paginationParams));
        }
    }
}