using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.AuthService
{
    public class AuthService(IUnitOfWork uow, IConfiguration config, ILogger<AuthService> logger) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IConfiguration _config = config;


        public async Task<ServiceResultDto<string>> Register(RegisterRequest request)
        {
            if (_unitOfWork.AuthRepository.CheckIfUsuarioExists(request.Login))
            {
                _logger.LogWarning("Usuário {usuario} já existe no banco de dados.", request.Login);
                return Conflict<string>("Usuário já existe");
            }
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var user = new Usuario { Login = request.Login, SenhaHash = hash };

            await _unitOfWork.AuthRepository.AddUsuarioAsync(user);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Usuário cadastrado.");
            return Ok("Usuario Cadastrado");
        }

        public async Task<ServiceResultDto<string>> Login(LoginRequest request)
        {

            var user = await _unitOfWork.AuthRepository.GetUsuarioByLoginAsync(request.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash))
            {
                 _logger.LogWarning("Credenciais incorretas.");
                return ValidationError<string>("Credenciais Incorretas.");
            }


            var claims = new[]{
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user")
            };
            var jwtSecret = _config["JWT_SECRET"];
            if (jwtSecret == null) return ServerError<string>("Serviço indisponível.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Token criado.");
            return Ok(jwt);
        }
    }
}