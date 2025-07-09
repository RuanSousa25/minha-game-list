using GamesList.Databases;
using GamesList.DTOs;

namespace GamesList.Services.ImagensService
{
    public interface IImagensService
    {   
          public Task<ServiceResultDto<string>> RemoveImagensByJogoId(int id);
    }
}