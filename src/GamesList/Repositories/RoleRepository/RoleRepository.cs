using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.RoleRepository
{
    public class RoleRepository(AppDbContext appDbContext) : IRoleRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        
        public async Task<Role?> GetRoleById(int id)
        {
            return await _appDbContext.Roles.FindAsync(id);
        }

        public async Task<Role?> GetRoleByRoleNome(string nome)
        {
            return await _appDbContext.Roles.SingleOrDefaultAsync(r => r.Nome == nome);
        }
    }
}