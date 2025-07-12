using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GenerosRepository;
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
        IGenerosRepository GenerosRepository {get; }
        IImagensRepository ImagensRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        IImagensSugestaoRepository ImagensSugestaoRepository { get; }
        ISugerirJogoRepository SugerirJogoRepository { get; }

        public Task CommitChangesAsync();
    }
}