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
using GamesList.Common.Pagination;
using System.Drawing;

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
            if (request.Nome.Length > 60)
            {
                _logger.LogWarning("Sugestão de jogo com mais de 60 caracteres. Usuário Id:{userId} ", userId);
                return BadRequest<int>("O nome do jogo não deve conter mais de 60 caracteres.");
            }
            if (request.Generos.Count > 6 && request.Generos.Count < 1)
            {
                _logger.LogWarning("Sugestão de jogo com quantidade inválida de gêneros cadastrados. Count: {count}", request.Generos.Count);
                return BadRequest<int>("O jogo deve conter entre 1 a 6 gêneros.");
            }
            var generos = await _unitOfWork.GeneroRepository.GetGenerosByGenerosIdsAsync([.. request.Generos],new PaginationParams());
            var sugestao = new SugestaoJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos.Items, DataSugestao = DateTime.UtcNow, Aprovado = false };
            await _unitOfWork.SugerirJogoRepository.AddSugestaoJogoAsync(sugestao);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Sugestão de jogo {nome} inserida com sucesso. Id da sugestão: {id}", sugestao.Nome, sugestao.Id);
            return Ok(sugestao.Id);
        }
        public async Task<ServiceResultDto<MessageResponseDto>> SaveSugestaoJogoComImagemAsync(UploadGameRequest request, IFormFile imagemCapa, IFormFile imagemIcone, int userId)
        {
            if (!ValidateExtesion(imagemIcone, true))
                return BadRequest<MessageResponseDto>("Ícone deve ser ICO, JPG ou PNG");

            if (!ValidateExtesion(imagemCapa, false))
                return BadRequest<MessageResponseDto>("Capa deve ser JPG ou PNG");

            var dimensionsCapa = GetDimensions(imagemCapa);
            if (dimensionsCapa == null || dimensionsCapa.Value.width != 460 || dimensionsCapa.Value.height != 215)
                return BadRequest<MessageResponseDto>("Capa deve ter 460x215px");

            var dimensionsIcone = GetDimensions(imagemIcone);
            if (dimensionsIcone == null || dimensionsIcone.Value.width > 512 || dimensionsIcone.Value.height > 512 || dimensionsIcone.Value.width / dimensionsIcone.Value.height != 1)
                return BadRequest<MessageResponseDto>("Ícone deve ter proporções de 1x1px");

            var sugestaoResult = await SaveSugestaoJogoAsync(request, userId);
            if (!sugestaoResult.Success) return ServerError<MessageResponseDto>(sugestaoResult.Message!);

            var imagemCapaResult = await SaveImagem(imagemCapa, 1, sugestaoResult.Data);
            if (!imagemCapaResult) _logger.LogError("Não foi possível inserir imagem de capa para o jogo {id}", sugestaoResult.Data);
            var imagemIconeResult = await SaveImagem(imagemIcone, 2, sugestaoResult.Data);
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

        public async Task<ServiceResultDto<PagedResult<SugestaoJogoDto>>> ListSugerirJogoPagedAsync(PaginationParams paginationParams)
        {
            return Ok(await _unitOfWork.SugerirJogoRepository.ListSugestoesJogosPagedAsync(paginationParams));
        }
        public async Task<ServiceResultDto<PagedResult<SugestaoJogoDto>>> ListSugerirJogoByUserIdPagedAsync(PaginationParams paginationParams, int userId)
        {
            return Ok(await _unitOfWork.SugerirJogoRepository.GetSugestoesJogoByUsuarioIdPagedAsync( paginationParams, userId));
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
        private static bool ValidateExtesion(IFormFile file, bool isIcon)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (isIcon)
            {
                var permitted = new[] { ".ico", ".jpg", ".jpeg", ".png" };
                return permitted.Contains(extension);
            }
            else
            {
                var permitted = new[] { ".jpg", ".jpeg", ".png" };
                return permitted.Contains(extension);
            }
        }
        private static (int width, int height)? GetDimensions(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            try
            {
                using var image = Image.FromStream(stream);
                return (image.Width, image.Height);
            }
            catch
            {
                return null;
            }
        }
    }
    

}