using GamesList.Models;

namespace GamesList.Repositories.ImagensSugestaoRepository
{
    public interface IImagensSugestaoRepository
    {
        public Task AddImagemAsync(ImagensSugestao imagem);
        public void RemoveSugestaoImagens(List<ImagensSugestao> imagens);
    }
}