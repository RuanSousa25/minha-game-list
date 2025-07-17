using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.ImagensSugestaoService
{
    public class ImagensSugestaoService(IUnitOfWork uow, ILogger<ImagensSugestaoService> logger) : IImagensSugestaoService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly ILogger<ImagensSugestaoService> _logger = logger;

        public async Task<ServiceResultDto<MessageResponseDto>> SaveImagemAsync(int sugestaoJogoId, string url)
        {
            var imagem = new ImagensSugestao { SugerirJogoId = sugestaoJogoId, Url = url, TipoId = 1 };
            await _unitOfWork.ImagensSugestaoRepository.AddImagemAsync(imagem);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Imagem {img} inserida com sucesso.", url);
            return Ok(new MessageResponseDto("Imagem inserida com sucesso"));
        }
        public ServiceResultDto<MessageResponseDto> RemoveSugestaoImagens(List<ImagensSugestao> imagens)
        {
                _unitOfWork.ImagensSugestaoRepository.RemoveSugestaoImagens(imagens);
                _logger.LogInformation("Removidas {count} sugestões de imagens.", imagens.Count);
                return Ok(new MessageResponseDto("Imagens removidas."));
        }
        public async Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(ImagensSugestao imagem)
        {
            await _unitOfWork.ImagensSugestaoRepository.AddImagemAsync(imagem);
            _logger.LogInformation("Imagem {id} adicionada do banco.", imagem.Id);
            return Ok(new MessageResponseDto("Imagem Adicionada com sucesso."));
        }
        
    }
}