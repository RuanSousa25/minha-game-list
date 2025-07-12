using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.GenerosRepository
{
    public class GenerosRepository(AppDbContext appDbContext) : IGenerosRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await _appDbContext.Generos.ToListAsync();
        }

        public async Task<List<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids)
        {
            return await _appDbContext.Generos.Where(g => ids.Contains(g.Id)).ToListAsync();
        }
    }
}