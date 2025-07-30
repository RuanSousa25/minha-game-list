using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.AuthRepository
{
    public class AuthRepository(AppDbContext appDbContext) : IAuthRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<Usuario> AddUsuarioAsync(Usuario user)
        {
            await _appDbContext.Usuarios.AddAsync(user);
            return user;
        }

        public bool CheckIfUsuarioExists(string login)
        {
            return _appDbContext.Usuarios.Any(u => u.Login == login);

        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _appDbContext.Usuarios.Include(u => u.Role).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetUsuarioByLoginAsync(string login)
        {
            var user = await _appDbContext.Usuarios.Include(u => u.Role).SingleOrDefaultAsync(u => u.Login == login);
            return user;
        }
    }
}