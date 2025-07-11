using GamesList.Models;

namespace GamesList.Repositories.ImagensRepository
{
    public interface IImagensRepository
    {
        public Task<List<Imagem>> GetImagensByJogoId(int id);
        public void RemoveImagens(List<Imagem> imagens);
        public Task AddImagemAsync(Imagem imagem);
    }
}