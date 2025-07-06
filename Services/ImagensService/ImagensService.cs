using GamesList.Databases;

namespace GamesList.Services.ImagensService
{
    public class ImagensServices(AppDbContext appDbContext, ILogger<ImagensServices> logger) : IImagensService
    {
         private readonly ILogger<ImagensServices> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;

        
    }
}