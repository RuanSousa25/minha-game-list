using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Databases
{

    public class GameDB(DbContextOptions<GameDB> options) : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();
    }
}