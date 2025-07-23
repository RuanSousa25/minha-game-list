using GamesList.Models;

namespace GamesList.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        public bool CheckIfUsuarioExists(string login);
        public Task<Usuario?> GetUsuarioByLoginAsync(string login);
        public Task<Usuario?> GetUsuarioByIdAsync(int id);
        public Task<Usuario> AddUsuarioAsync(Usuario user);
    }
}