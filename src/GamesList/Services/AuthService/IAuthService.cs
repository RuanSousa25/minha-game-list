using GamesList.DTOs;
using GamesList.DTOs.Requests;

namespace GamesList.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResultDto<string>> Register(RegisterRequest request);
        public Task<ServiceResultDto<string>> Login(LoginRequest request);

    }
}