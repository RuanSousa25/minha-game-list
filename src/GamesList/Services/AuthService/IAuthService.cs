using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResultDto<MessageResponseDto>> RegisterAsync(RegisterRequest request);
        public Task<ServiceResultDto<LoginResponseDto>> LoginAsync(LoginRequest request);
        public Task<ServiceResultDto<Usuario>> GetUsuarioByIdAsync(int id);
        public Task<ServiceResultDto<UsuarioDto>> GetUsuarioDtoByIdAsync(int id);
    }
}