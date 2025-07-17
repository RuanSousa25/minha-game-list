using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;

namespace GamesList.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResultDto<MessageResponseDto>> RegisterAsync(RegisterRequest request);
        public Task<ServiceResultDto<LoginResponseDto>> LoginAsync(LoginRequest request);

    }
}