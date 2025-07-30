using GamesList.Databases;
using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GeneroRepository;
using GamesList.Repositories.ImagensRepository;
using GamesList.Repositories.SugestoesImagemRepository;
using GamesList.Repositories.JogoRepository;
using GamesList.Repositories.SugestoesJogoRepository;
using GamesList.Repositories.RoleRepository;

namespace GamesList.Repositories.UnitOfWork
{
    public class UnitOfWork(
        AppDbContext appDbContext,
        IRoleRepository roleRepo,
        IAuthRepository authRepo,
        IJogoRepository jogoRepo,
        IGeneroRepository genRepo,
        IImagensRepository imRepo,
        IAvaliacaoRepository avRepo,
        ISugestoesJogoRepository SugRepo,
        ISugestoesImagemRepository imSRepo
        ) : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public IRoleRepository RoleRepository { get; } = roleRepo;
        public IAuthRepository AuthRepository { get; } = authRepo;
        public IJogoRepository JogoRepository { get; } = jogoRepo;
        public IGeneroRepository GeneroRepository { get; } = genRepo;
        public IImagensRepository ImagensRepository { get; } = imRepo;
        public IAvaliacaoRepository AvaliacaoRepository { get; } = avRepo;
        public ISugestoesImagemRepository SugestoesImagemRepository { get; } = imSRepo;
        public ISugestoesJogoRepository SugerirJogoRepository { get; } = SugRepo;


        public async Task CommitChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}