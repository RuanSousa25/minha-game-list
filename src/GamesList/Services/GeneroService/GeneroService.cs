using GamesList.DTOs;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using static GamesList.DTOs.Helpers.Results;
namespace GamesList.Services.GeneroService
{
    public class GeneroService(IUnitOfWork uow, ILogger<GeneroService> logger) : IGeneroService
    {
        private ILogger<GeneroService> _logger = logger;
        private IUnitOfWork _unitOfWork = uow;

        public async Task<ServiceResultDto<List<Genero>>> ListGenerosAsync()
        {
            return Ok(await _unitOfWork.GeneroRepository.GetGenerosAsync());
        }

        public async Task<ServiceResultDto<List<Genero>>> ListGenerosByIdsAsync(List<int> ids)
        {
            return Ok(await _unitOfWork.GeneroRepository.GetGenerosByGenerosIdsAsync(ids));
        }
    }
}