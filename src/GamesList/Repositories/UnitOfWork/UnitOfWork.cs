using GamesList.Databases;
using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GeneroRepository;
using GamesList.Repositories.ImagensRepository;
using GamesList.Repositories.ImagensSugestaoRepository;
using GamesList.Repositories.JogoRepository;
using GamesList.Repositories.SugerirJogoRepository;

namespace GamesList.Repositories.UnitOfWork
{
    public class UnitOfWork(
        AppDbContext appDbContext,
        IAuthRepository authRepo,
        IJogoRepository jogoRepo,
        IGeneroRepository genRepo,
        IImagensRepository imRepo,
        IAvaliacaoRepository avRepo,
        ISugerirJogoRepository SugRepo,
        IImagensSugestaoRepository imSRepo
        ) : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public IAuthRepository AuthRepository { get; } = authRepo;
        public IJogoRepository JogoRepository { get; } = jogoRepo;
        public IGeneroRepository GeneroRepository { get; } = genRepo;
        public IImagensRepository ImagensRepository { get; } = imRepo;
        public IAvaliacaoRepository AvaliacaoRepository { get; } = avRepo;
        public IImagensSugestaoRepository ImagensSugestaoRepository { get; } = imSRepo;
        public ISugerirJogoRepository SugerirJogoRepository { get; } = SugRepo;


        public async Task CommitChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}