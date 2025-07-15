using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GeneroRepository;
using GamesList.Repositories.ImagensRepository;
using GamesList.Repositories.ImagensSugestaoRepository;
using GamesList.Repositories.JogoRepository;
using GamesList.Repositories.SugerirJogoRepository;

namespace GamesList.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IJogoRepository JogoRepository { get; }
        IGeneroRepository GeneroRepository {get; }
        IImagensRepository ImagensRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        IImagensSugestaoRepository ImagensSugestaoRepository { get; }
        ISugerirJogoRepository SugerirJogoRepository { get; }

        public Task CommitChangesAsync();
    }
}