using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Models;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.JogoService;
using Microsoft.Extensions.Logging;
using Moq;

namespace GamesList.Tests.Services
{
    public class AvaliacaoServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IAvaliacaoRepository> _mockAvaliacaoRepo;
        private readonly Mock<IJogoService> _mockJogoService;
        private readonly Mock<ILogger<AvaliacaoService>> _mockLogger;
        private readonly IAvaliacaoService _service;
        public AvaliacaoServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockAvaliacaoRepo = new Mock<IAvaliacaoRepository>();
            _mockJogoService = new Mock<IJogoService>();
            _mockLogger = new Mock<ILogger<AvaliacaoService>>();

            _mockUow.Setup(u => u.AvaliacaoRepository).Returns(_mockAvaliacaoRepo.Object);
            _service = new AvaliacaoService(_mockUow.Object, _mockJogoService.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task RemoveAvaliacao_UsuarioNaoAutorizado_DeveNegarRemocao()
        {
            int userId = 10;
            var avaliacao = new Avaliacao
            {
                Id = 12,
                UsuarioId = 5,
                JogoId = 2,
                Nota = 7,
                Opiniao = "Jogo melhor do que o esperado",
                Data = DateTime.UtcNow.AddDays(-5)
            };

            _mockAvaliacaoRepo.Setup(a => a.GetAvaliacaoByIdAsync(avaliacao.Id)).ReturnsAsync(avaliacao);


            var result = await _service.RemoveAvaliacaoByIdAsync(avaliacao.Id, userId, false);

            Assert.False(result.Success);
            Assert.Equal(403, result.StatusCode);
            Assert.Contains("permissão", result.Message);
            Assert.Null(result.Data);

            _mockAvaliacaoRepo.Verify(a => a.RemoveAvaliacao(avaliacao), Times.Never);
            _mockUow.Verify(u => u.CommitChangesAsync(), Times.Never);

        }
        [Fact]
        public async Task SaveAvalicao_UsuarioJaAvaliou_DeveAtualizarAvaliacaoExistente()
        {
            int userId = 1;
            var request = new AvaliacaoRequest { JogoId = 10, Nota = 0, Opiniao = "ótimo jogo. Nova opinião." };

            var avaliacaoExistente = new Avaliacao
            {
                UsuarioId = userId,
                JogoId = request.JogoId,
                Nota = 4,
                Opiniao = "Não gostei tanto. Opinião antiga",
                Data = DateTime.UtcNow.AddDays(-1)
            };

            _mockJogoService
            .Setup(j => j.CheckIfJogoExistsAsync(request.JogoId))
            .ReturnsAsync(new ServiceResultDto<bool> { Success = true, Data = true });

            _mockAvaliacaoRepo
            .Setup(a => a.GetAvaliacaoByUsuarioIdAndJogoIdAsync(userId, request.JogoId))
            .ReturnsAsync(avaliacaoExistente);


            _mockUow.Setup(u => u.CommitChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.SaveAvaliacaoAsync(userId, request);

            Assert.True(result.Success);
            _mockAvaliacaoRepo
            .Verify(
                a =>
                a.UpdateAvaliacao(
                    It.Is<Avaliacao>(
                        av =>
                        av.Id == avaliacaoExistente.Id
                         && av.Nota == request.Nota
                         && av.Opiniao == request.Opiniao
                         ))
            , Times.Once);

            _mockAvaliacaoRepo.Verify(
                a => a.AddAvaliacaoAsync(It.IsAny<Avaliacao>()),
                Times.Never
            );

            _mockUow.Verify(u => u.CommitChangesAsync(), Times.Once);
        }

    }
}