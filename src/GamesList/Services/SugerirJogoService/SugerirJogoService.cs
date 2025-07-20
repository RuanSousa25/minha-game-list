using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.BlobService;
using GamesList.Services.ImagensService;
using GamesList.Services.ImagensSugestaoService;
using GamesList.Services.JogoService;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.SugerirJogoService
{
    public class SugerirJogoService(IUnitOfWork uow,
    IJogoService jogoService,
     IBlobService blobService,
      IImagensSugestaoService imagensSugestaoService,
       IImagensService imagensService,
        ILogger<SugerirJogoService> logger) : ISugerirJogoService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IJogoService _jogoService = jogoService;
        private readonly IBlobService _blobService = blobService;
        private readonly IImagensSugestaoService _imagensSugestaoService = imagensSugestaoService;
        private readonly IImagensService _imagensService = imagensService;
        private readonly ILogger<SugerirJogoService> _logger = logger;

        public async Task<ServiceResultDto<int>> SaveSugestaoJogoAsync(UploadGameRequest request, int userId)
        {
            var generos = await _unitOfWork.GeneroRepository.GetGenerosByGenerosIdsAsync([.. request.Generos]);
            var sugestao = new SugerirJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos, DataSugestao = DateTime.UtcNow, Aprovado = false };
            await _unitOfWork.SugerirJogoRepository.AddSugerirJogoAsync(sugestao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de jogo {nome} inserida com sucesso. Id da sugestão: {id}", sugestao.Nome, sugestao.Id);
            return Ok(sugestao.Id);
        }
        public async Task<ServiceResultDto<MessageResponseDto>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagem, int userId)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
            var blobResult = await _blobService.UploadFileAsync(imagem.OpenReadStream(), fileName, imagem.ContentType);
            if (!blobResult.Success)
                return new ServiceResultDto<MessageResponseDto>()
                {
                    StatusCode = blobResult.StatusCode,
                    Message = blobResult.Message,
                    Success = blobResult.Success
                };

            var sugestaoResult = await SaveSugestaoJogoAsync(request, userId);
            if (!sugestaoResult.Success) return ServerError<MessageResponseDto>("Não foi possível inserir a sugestão.");

            var sugestaoImagemResult = await _imagensSugestaoService.SaveImagemAsync(sugestaoResult.Data, blobResult.Data.Url!);
            if (!sugestaoImagemResult.Success) return sugestaoImagemResult;

            return Created(new MessageResponseDto("Sugestão inserida com sucesso"));
        }

        public async Task<ServiceResultDto<JogoDto>> AprovarJogoAsync(int id)
        {
            var sugestao =
            await _unitOfWork.SugerirJogoRepository.GetSugerirJogoComRelacoesByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Não foi encontrada a sugestão de {id}", id);
                return NotFound<JogoDto>("Sugestão não encontrada");
            }
            if (sugestao.Aprovado)
            {
                _logger.LogWarning("Sugestão já aprovada {id}", id);
                return BadRequest<JogoDto>("Sugestão já aprovada");
            }
            sugestao.Aprovado = true;

            var jogo = new Jogo { Generos = [.. sugestao.Generos], Nome = sugestao.Nome };
            await _jogoService.AddJogoAsync(jogo);
            await _unitOfWork.CommitChangesAsync();

            var imagensSugestoes = sugestao.Imagens;
            foreach (var imagemSugestao in imagensSugestoes)
            {
                await _imagensService.AddImagemAsync(new Imagem { Url = imagemSugestao.Url, JogoId = jogo.Id, TipoId = imagemSugestao.TipoId });
            }
             await _unitOfWork.CommitChangesAsync();
            return Ok(new JogoDto(jogo));
        }

        public async Task<ServiceResultDto<List<SugerirJogoDto>>> ListSugerirJogoAsync()
        {
            var sugestoes = await _unitOfWork.SugerirJogoRepository.ListSugerirJogosAsync();
            return Ok(sugestoes.Select(s => new SugerirJogoDto(s)).ToList());
        }
        public async Task<ServiceResultDto<MessageResponseDto>> RemoverSugestaoJogoAsync(int id)
        {
            var sugestao = await _unitOfWork.SugerirJogoRepository.GetSugerirJogoComRelacoesByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Sugestão de Id {id} não encontrada.", id);
                return NotFound<MessageResponseDto>("Sugestão não encontrada.");
            }
            _unitOfWork.SugerirJogoRepository.RemoveSugestao(sugestao);
            _imagensSugestaoService.RemoveSugestaoImagens([.. sugestao.Imagens]);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de Id {id} foi removida com sucesso.", id);
            return Ok(new MessageResponseDto("Sugestão removida com sucesso"));
        }
        public async Task<ServiceResultDto<SugerirJogo>> FindSugestaoJogoAsync(int id)
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