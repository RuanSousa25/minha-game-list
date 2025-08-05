using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Models;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AuthService;
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
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AvaliacaoService>> _mockLogger;
        private readonly IAvaliacaoService _service;
        public AvaliacaoServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockAvaliacaoRepo = new Mock<IAvaliacaoRepository>();
            _mockJogoService = new Mock<IJogoService>();
            _mockAuthService = new Mock<IAuthService> ();
            _mockLogger = new Mock<ILogger<AvaliacaoService>>();

            _mockUow.Setup(u => u.AvaliacaoRepository).Returns(_mockAvaliacaoRepo.Object);
            _service = new AvaliacaoService(_mockUow.Object, _mockJogoService.Object, _mockAuthService.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task RemoveAvaliacao_UsuarioNaoAutorizado_DeveNegarRemocao()
        {
            var usuarioRequisitanteId = 2;
            var usuarioAvaliador = new Usuario
            {
                Id = 10,
                Login = "mockUsuario",
                Role = new Role
                {
                    Id = 1,
                    Nome = "usuario"
                },
                SenhaHash = "hash"
            };
            var jogo = new Jogo
            {
                Id = 1,
                Nome = "mockJogo",
            };
            var avaliacao = new Avaliacao
            {
                Id = 12,
                Usuario = usuarioAvaliador,
                Jogo = jogo,
                Nota = 7,
                Opiniao = "Jogo melhor do que o esperado",
                DataCriacao = DateTime.UtcNow.AddDays(-5)
            };

            _mockAvaliacaoRepo.Setup(a => a.GetAvaliacaoByIdAsync(avaliacao.Id)).ReturnsAsync(avaliacao);


            var result = await _service.RemoveAvaliacaoByIdAsync(avaliacao.Id, usuarioRequisitanteId, false);

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
            var usuarioRequisitanteId = 10;
            var usuarioAvaliador = new Usuario
            {
                Id = 10,
                Login = "mockUsuario",
                Role = new Role
                {
                    Id = 1,
                    Nome = "usuario"
                },
                SenhaHash = "hash"
            };
            var jogo = new Jogo
            {
                Id = 1,
                Nome = "mockJogo",
            };

            var request = new AvaliacaoRequest { JogoId = 1, Nota = 0, Opiniao = "ótimo jogo. Nova opinião." };

            var avaliacaoExistente = new Avaliacao
            {
                Usuario = usuarioAvaliador,
                Jogo = jogo,
                Nota = 4,
                Opiniao = "Não gostei tanto. Opinião antiga",
                DataCriacao = DateTime.UtcNow.AddDays(-1),
            };

            _mockJogoService
            .Setup(j => j.GetJogoAsync(request.JogoId))
            .ReturnsAsync(new ServiceResultDto<Jogo>(){ Data = jogo, Success = true, StatusCode = 200});

            _mockAuthService
            .Setup(a => a.GetUsuarioByIdAsync(usuarioRequisitanteId))
            .ReturnsAsync(new ServiceResultDto<Usuario>() { Data = usuarioAvaliador, Success = true, StatusCode = 200 });

            _mockAvaliacaoRepo
            .Setup(a => a.GetAvaliacaoByUsuarioIdAndJogoIdAsync(usuarioRequisitanteId, request.JogoId))
            .ReturnsAsync(avaliacaoExistente);


            _mockUow.Setup(u => u.CommitChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.SaveAvaliacaoAsync(usuarioRequisitanteId, request);

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