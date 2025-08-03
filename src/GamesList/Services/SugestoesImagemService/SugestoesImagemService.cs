using GamesList.Dtos;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.SugestoesImagemService
{
    public class SugestoesImagemService(IUnitOfWork uow, ILogger<SugestoesImagemService> logger) : ISugestoesImagemService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly ILogger<SugestoesImagemService> _logger = logger;

        public async Task<ServiceResultDto<MessageResponseDto>> SaveImagemAsync(int sugestaoJogoId, string url, int tipoId)
        {
            var imagem = new SugestaoImagem { SugestaoJogoId = sugestaoJogoId, Url = url, TipoId = tipoId };
            await _unitOfWork.SugestoesImagemRepository.AddImagemAsync(imagem);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Imagem {img} inserida com sucesso.", url);
            return Ok(new MessageResponseDto("Imagem inserida com sucesso"));
        }
        public ServiceResultDto<MessageResponseDto> RemoveSugestoesImagem(List<SugestaoImagem> imagens)
        {
                _unitOfWork.SugestoesImagemRepository.RemoveSugestaoImagens(imagens);
                _logger.LogInformation("Removidas {count} sugestões de imagens.", imagens.Count);
                return Ok(new MessageResponseDto("Imagens removidas."));
        }
        public async Task<ServiceResultDto<MessageResponseDto>> AddImagemAsync(SugestaoImagem imagem)
        {
            await _unitOfWork.SugestoesImagemRepository.AddImagemAsync(imagem);
            _logger.LogInformation("Imagem {id} adicionada do banco.", imagem.Id);
            return Ok(new MessageResponseDto("Imagem Adicionada com sucesso."));
        }
        
    }
}