using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.BlobService;
using GamesList.Services.ImagensSugestaoService;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.SugerirJogoService
{
    public class SugerirJogoService(IUnitOfWork uow, IBlobService blobService, IImagensSugestaoService imagensServices, ILogger<SugerirJogoService> logger) : ISugerirJogoService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IBlobService _blobService = blobService;
        private readonly IImagensSugestaoService _imagensServices = imagensServices;
        private readonly ILogger<SugerirJogoService> _logger = logger;

        public async Task<ServiceResultDto<int>> SaveSugestaoJogo(UploadGameRequest request, int userId)
        {
            var generos = await _unitOfWork.GenerosRepository.GetGenerosByGenerosIdsAsync([.. request.Generos]);
            var sugestao = new SugerirJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos, DataSugestao = DateTime.UtcNow, Aprovado = false };
            await _unitOfWork.SugerirJogoRepository.AddSugerirJogoAsync(sugestao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de jogo {nome} inserida com sucesso. Id da sugestão: {id}", sugestao.Nome, sugestao.Id);
            return Ok(sugestao.Id);
        }
        public async Task<ServiceResultDto<string>> SaveSugestaoJogoComImagem(UploadGameRequest request, IFormFile imagem, int userId)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
            var blobResult = await _blobService.UploadFileAsync(imagem.OpenReadStream(), fileName, imagem.ContentType);
            if (!blobResult.Success) return blobResult;

            var sugestaoResult = await SaveSugestaoJogo(request, userId);
            if (!sugestaoResult.Success) return ServerError<string>("Não foi possível inserir a sugestão.");

            var sugestaoImagemResult = await _imagensServices.SaveImagem(sugestaoResult.Data, blobResult.Data!);
            if (!sugestaoImagemResult.Success) return sugestaoImagemResult;

            return Ok("Sugestão inserida com sucesso");
        }

        public async Task<ServiceResultDto<JogoDTO>> AprovarJogo(int id)
        {
            var sugestao =
            await _unitOfWork.SugerirJogoRepository.GetSugerirJogoComRelacoesByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Não foi encontrada a sugestão de {id}", id);
                return NotFound<JogoDTO>("Sugestão não encontrada.");
            }
            sugestao.Aprovado = true;

            var jogo = new Jogo { Generos = [.. sugestao.Generos], Nome = sugestao.Nome };
            await _unitOfWork.JogoRepository.AddJogoAsync(jogo); //criar service dedicado
           

            var imagens = sugestao.Imagens;
            foreach (var imagem in imagens)
            {
                await _unitOfWork.ImagensRepository.AddImagemAsync(new Imagem { Url = imagem.Url, JogoId = jogo.Id, TipoId = imagem.TipoId });
            }
             await _unitOfWork.CommitChangesAsync();
            return Ok(new JogoDTO(jogo));
        }

        public async Task<ServiceResultDto<List<SugerirJogo>>> ListSugerirJogo()
        {
            var sugestoes = await _unitOfWork.SugerirJogoRepository.ListSugerirJogosAsync();
            return Ok(sugestoes);
        }
        public async Task<ServiceResultDto<string>> RemoverSugestaoJogo(int id)
        {
            var sugestao = await _unitOfWork.SugerirJogoRepository.GetSugerirJogoComRelacoesByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Sugestão de Id {id} não encontrada.", id);
                return NotFound<string>("Sugestão não encontrada.");
            }
            _unitOfWork.SugerirJogoRepository.RemoveSugestao(sugestao);
            _unitOfWork.ImagensSugestaoRepository.RemoveSugestaoImagens([.. sugestao.Imagens]); //criar service dedicado
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de Id {id} foi removida com sucesso.", id);
            return Ok("Sugestão removida com sucesso.");
        }
        public async Task<ServiceResultDto<SugerirJogo>> FindSugestaoJogo(int id)
        {
            var sugestao = await _unitOfWork.SugerirJogoRepository.GetSugerirJogoByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Sugestão de Id {id} não encontrada.", id);
                return NotFound<SugerirJogo>("Sugestão não encontrada");
            }
            return Ok(sugestao);
        } 
    }
}