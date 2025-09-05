using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.GeneroRepository
{
    public class GeneroRepository(AppDbContext appDbContext) : IGeneroRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Genero>> GetGenerosAsync(string? search)
        {
            return await _appDbContext.Generos.Where(g => search == null || g.Nome.Contains(search)).ToListAsync();
        }

        public async Task<List<Genero>> GetGenerosByGenerosIdsAsync(List<int> ids)
        {
            return await _appDbContext.Generos.Where(g => ids.Contains(g.Id)).ToListAsync();
        }
    }
}