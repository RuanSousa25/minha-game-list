using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;
namespace GamesList.Services
{
    public class SugerirJogoService(AppDbContext appDbContext, BlobService blobService, ImagensSugestaoService imagensServices, ILogger<SugerirJogoService> logger)
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly BlobService _blobService = blobService;
        private readonly ImagensSugestaoService _imagensServices = imagensServices;
        private readonly ILogger<SugerirJogoService> _logger = logger;

        public async Task<ServiceResultDto<int>> SaveSugestaoJogo(UploadGameRequest request, int userId)
        {
            var generos = _appDbContext.Generos.Where(g => request.Generos.Contains(g.Id)).ToList();
            var sugestao = new SugerirJogo { UsuarioId = userId, Nome = request.Nome, Generos = generos, DataSugestao = DateTime.UtcNow, Aprovado = false };
            _appDbContext.SugerirJogo.Add(sugestao);
            await _appDbContext.SaveChangesAsync();
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
            await _appDbContext.SugerirJogo
            .Include(s => s.Generos)
            .Include(s => s.Imagens)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (sugestao == null)
            {
                _logger.LogWarning("Não foi encontrada a sugestão de {id}", id);
                return NotFound<JogoDTO>("Sugestão não encontrada.");
            }
            sugestao.Aprovado = true;

            var jogo = new Jogo { Generos = sugestao.Generos.ToList(), Nome = sugestao.Nome };
            await _appDbContext.Jogos.AddAsync(jogo);
            await _appDbContext.SaveChangesAsync();

            var imagens = sugestao.Imagens;
            foreach (var imagem in imagens)
            {
                await _appDbContext.Imagens.AddAsync(new Imagem { Url = imagem.Url, JogoId = jogo.Id, TipoId = imagem.TipoId });
            }
            await _appDbContext.SaveChangesAsync();
            return Ok(new JogoDTO(jogo));
        }

        internal async Task<ServiceResultDto<List<SugerirJogo>>> ListSugerirJogo()
        {
            var sugestoes =
            await _appDbContext.SugerirJogo
            .Include(s => s.Generos)
            .Include(s => s.Imagens)
            .ToListAsync();

            return Ok(sugestoes);
        }
    }
}