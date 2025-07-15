using GamesList.DTOs;
using GamesList.DTOs.Requests;

namespace GamesList.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResultDto<string>> RegisterAsync(RegisterRequest request);
        public Task<ServiceResultDto<string>> LoginAsync(LoginRequest request);

    }
}