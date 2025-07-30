using GamesList.Models;

namespace GamesList.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        public Task<Role?> GetRoleById(int id);
        public Task<Role?> GetRoleByRoleNome(string nome);   
    }
}