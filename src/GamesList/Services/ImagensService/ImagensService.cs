using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.BlobService;
using Microsoft.EntityFrameworkCore;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.ImagensService
{
    public class ImagensServices(IUnitOfWork uow, IBlobService blobService, ILogger<ImagensServices> logger) : IImagensService
    {
        private readonly ILogger<ImagensServices> _logger = logger;
        private readonly IBlobService _blobService = blobService;
        private readonly IUnitOfWork _unitOfWork = uow;

        public async Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(Imagem imagem)
        {
            await _unitOfWork.ImagensRepository.AddImagemAsync(imagem);
            return Ok(new MessageResponseDto("Imagem adicionada com sucesso"));
        }

        public async Task<ServiceResultDto<MessageResponseDto>> RemoveImagensByJogoIdAsync(int id)
        {
            var imagens = await _unitOfWork.ImagensRepository.GetImagensByJogoId(id);

            _unitOfWork.ImagensRepository.RemoveImagens(imagens);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Realizada a remoção de {lenght} imagens do jogo {id}", imagens.Count, id);

            var deleteTasks = imagens.Select(async i => await _blobService.DeleteFileAsync(i.Url));
            await Task.WhenAll(deleteTasks);
            
            return Ok(new MessageResponseDto("Remoção de imagens realizada realizada."));

        }
    }
}