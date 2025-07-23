using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;
using GamesList.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using static GamesList.Dtos.Helpers.Results;

namespace GamesList.Services.AuthService
{
    public partial class AuthService(IUnitOfWork uow, IConfiguration config, ILogger<AuthService> logger) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = uow;
        private readonly IConfiguration _config = config;


        public async Task<ServiceResultDto<MessageResponseDto>> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Senha))
            {
                _logger.LogWarning("Login ou senha vazios");
                return BadRequest<MessageResponseDto>("Credenciais inválidas ou ausentes");
            }
            if (!SenhaValidaRegex().IsMatch(request.Senha))
            {
                _logger.LogWarning("Login ou senha vazios");
                return ValidationError<MessageResponseDto>("A senha deve conter, pelo menos, 8 caraceteres, com letras e números");
            }
            if (_unitOfWork.AuthRepository.CheckIfUsuarioExists(request.Login))
            {
                _logger.LogWarning("Usuário {usuario} já existe no banco de dados.", request.Login);
                return Conflict<MessageResponseDto>("Usuário já existe");
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var user = new Usuario { Login = request.Login, SenhaHash = hash };

            await _unitOfWork.AuthRepository.AddUsuarioAsync(user);
            await _unitOfWork.CommitChangesAsync();
            _logger.LogInformation("Usuário cadastrado.");
            return Created(new MessageResponseDto("Usuário cadastrado com sucesso"));
        }

        public async Task<ServiceResultDto<LoginResponseDto>> LoginAsync(LoginRequest request)
        {

            var user = await _unitOfWork.AuthRepository.GetUsuarioByLoginAsync(request.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash))
            {
                _logger.LogWarning("Credenciais incorretas.");
                return Unauthorized<LoginResponseDto>("Credenciais Incorretas.");
            }


            var claims = new[]{
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user")
            };
            var jwtSecret = _config["JWT_SECRET"];
            if (jwtSecret == null) return ServerError<LoginResponseDto>("Serviço indisponível.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Token criado.");
            return Ok(new LoginResponseDto(jwt));
        }

        [GeneratedRegex("^(?=.*[A-Za-z])(?=.*\\d).{8,}$")]
        private static partial Regex SenhaValidaRegex();

        public async Task<ServiceResultDto<Usuario>> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _unitOfWork.AuthRepository.GetUsuarioByIdAsync(id);
            if (usuario == null) return NotFound<Usuario>("Usuário não encontrado");
            return Ok(usuario);
        }
    }
}