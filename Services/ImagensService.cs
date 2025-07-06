using GamesList.Databases;

namespace GamesList.Services
{
    public class ImagensServices(AppDbContext appDbContext, ILogger<ImagensServices> logger)
    {
         private readonly ILogger<ImagensServices> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;

        
    }
}