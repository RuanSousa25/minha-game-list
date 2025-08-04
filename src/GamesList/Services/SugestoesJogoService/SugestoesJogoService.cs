using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.BlobService;
using GamesList.Services.ImagensService;
using GamesList.Services.SugestoesImagemService;
using GamesList.Services.JogoService;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.SugestoesJogoService
{
    public class SugestoesJogoService(IUnitOfWork uow,
    IJogoService jogoService,
     IBlobService blobService,
      ISugestoesImagemService sugestoesImagemService,
       IImagensService imagensService,
        ILogger<SugestoesJogoService> logger) : ISugestoesJogoService
    {
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IJogoService _jogoService = jogoService;
        private readonly IBlobService _blobService = blobService;
        private readonly ISugestoesImagemService _sugestoesImagemService = sugestoesImagemService;
        private readonly IImagensService _imagensService = imagensService;
        private readonly ILogger<SugestoesJogoService> _logger = logger;

        public async Task<ServiceResultDto<int>> SaveSugestaoJogoAsync(UploadGameRequest request, int userId)
        {
            var generos = await _unitOfWork.GeneroRepository.GetGenerosByGenerosIdsAsync([.. request.Generos]);
            var sugestao = new SugestaoJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos, DataSugestao = DateTime.UtcNow, Aprovado = false };
            await _unitOfWork.SugerirJogoRepository.AddSugestaoJogoAsync(sugestao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de jogo {nome} inserida com sucesso. Id da sugestão: {id}", sugestao.Nome, sugestao.Id);
            return Ok(sugestao.Id);
        }
        public async Task<ServiceResultDto<MessageResponseDto>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagemCapa, IFormFile imagemIcone, int userId)
        {

            var sugestaoResult = await SaveSugestaoJogoAsync(request, userId);
            if (!sugestaoResult.Success) return ServerError<MessageResponseDto>("Não foi possível inserir a sugestão.");

            var imagemCapaResult = await SaveImagem(imagemCapa, 1,sugestaoResult.Data);
            if (!imagemCapaResult) _logger.LogError("Não foi possível inserir imagem de capa para o jogo {id}", sugestaoResult.Data);
            var imagemIconeResult = await SaveImagem(imagemIcone, 2,sugestaoResult.Data);
            if (!imagemIconeResult) _logger.LogError("Não foi possível inserir imagem de capa para o jogo {id}", sugestaoResult.Data);

            return Created(new MessageResponseDto("Sugestão inserida com sucesso"));
        }
        private async Task<bool> SaveImagem(IFormFile imagem, int tipoId, int jogoId)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
            var blobResult = await _blobService.UploadFileAsync(imagem.OpenReadStream(), fileName, imagem.ContentType);
            if (!blobResult.Success) return false;
            var sugestaoImagemResult = await _sugestoesImagemService.SaveImagemAsync(jogoId, blobResult.Data.Url!, tipoId);
            if (!sugestaoImagemResult.Success) return false;

            return true;

        }

        public async Task<ServiceResultDto<JogoDto>> AprovarJogoAsync(int id, int usuarioId)
        {
            var sugestao =
            await _unitOfWork.SugerirJogoRepository.GetSugestaoJogoComRelacoesByIdAsync(id);
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
           

            var jogo = new Jogo { Generos = [.. sugestao.Generos], Nome = sugestao.Nome };
            await _jogoService.AddJogoAsync(jogo);
            await _unitOfWork.CommitChangesAsync();

            sugestao.Aprovado = true;
            sugestao.DataAprovacao = DateTime.UtcNow;
            sugestao.JogoAprovadoId = jogo.Id;
            sugestao.AprovadorId = usuarioId;

            var imagensSugestoes = sugestao.Imagens;
            foreach (var imagemSugestao in imagensSugestoes)
            {
                await _imagensService.AddImagemAsync(new Imagem { Url = imagemSugestao.Url, JogoId = jogo.Id, TipoId = imagemSugestao.TipoId });
            }
            await _unitOfWork.CommitChangesAsync();
            return Ok(new JogoDto(jogo));
        }

        public async Task<ServiceResultDto<List<SugestaoJogoDto>>> ListSugerirJogoAsync()
        {
            var sugestoes = await _unitOfWork.SugerirJogoRepository.ListSugestoesJogosAsync();
            return Ok(sugestoes.Select(s => new SugestaoJogoDto(s)).ToList());
        }
        public async Task<ServiceResultDto<MessageResponseDto>> RemoverSugestaoJogoAsync(int id)
        {
            var sugestao = await _unitOfWork.SugerirJogoRepository.GetSugestaoJogoComRelacoesByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Sugestão de Id {id} não encontrada.", id);
                return NotFound<MessageResponseDto>("Sugestão não encontrada.");
            }
            _unitOfWork.SugerirJogoRepository.RemoveSugestao(sugestao);
            _sugestoesImagemService.RemoveSugestoesImagem([.. sugestao.Imagens]);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de Id {id} foi removida com sucesso.", id);
            return Ok(new MessageResponseDto("Sugestão removida com sucesso"));
        }
        public async Task<ServiceResultDto<SugestaoJogo>> FindSugestaoJogoAsync(int id)
        {
            var sugestao = await _unitOfWork.SugerirJogoRepository.GetSugestaoJogoByIdAsync(id);
            if (sugestao == null)
            {
                _logger.LogWarning("Sugestão de Id {id} não encontrada.", id);
                return NotFound<SugestaoJogo>("Sugestão não encontrada");
            }
            return Ok(sugestao);
        } 
    }
}