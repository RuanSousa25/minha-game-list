using GamesList.Databases;
using GamesList.DTOs;

namespace GamesList.Services
{
    public class AvaliacaoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;


    }
}