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
    public interface IUnitOfWork
    {
        IRoleRepository RoleRepository { get; }
        IAuthRepository AuthRepository { get; }
        IJogoRepository JogoRepository { get; }
        IGeneroRepository GeneroRepository {get; }
        IImagensRepository ImagensRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        ISugestoesImagemRepository SugestoesImagemRepository { get; }
        ISugestoesJogoRepository SugerirJogoRepository { get; }

        public Task CommitChangesAsync();
    }
}