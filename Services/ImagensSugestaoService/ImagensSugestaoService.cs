using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.ImagensSugestaoService
{
    public class ImagensSugestaoService(IUnitOfWork uow, ILogger<ImagensSugestaoService> logger) : IImagensSugestaoService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly ILogger<ImagensSugestaoService> _logger = logger;

        public async Task<ServiceResultDto<string>> SaveImagem(int sugestaoJogoId, string url)
        {
            var imagem = new ImagensSugestao { SugerirJogoId = sugestaoJogoId, Url = url, TipoId = 1 };
            await _unitOfWork.ImagensSugestaoRepository.AddImagemAsync(imagem);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Imagem {img} inserida com sucesso.", url);
            return Ok("Imagem inserida com sucesso");
        }
        
    }
}