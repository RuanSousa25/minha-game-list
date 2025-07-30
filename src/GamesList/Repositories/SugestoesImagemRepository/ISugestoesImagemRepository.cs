using GamesList.Models;

namespace GamesList.Repositories.SugestoesImagemRepository
{
    public interface ISugestoesImagemRepository
    {
        public Task AddImagemAsync(SugestaoImagem imagem);
        public void RemoveSugestaoImagens(List<SugestaoImagem> imagens);
    }
}