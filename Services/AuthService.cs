using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services
{
    public class AuthService(AppDbContext appDbContext, IConfiguration config)
    {
        private AppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _config = config;

        public async Task<ServiceResultDto<string>> Register(RegisterRequest request)
        {
            if (_appDbContext.Usuarios.Any(u => u.Login == request.Login)) return Conflict<string>("Usuário já existe");
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var user = new Usuario { Login = request.Login, SenhaHash = hash };

            _appDbContext.Usuarios.Add(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Usuario Cadastrado");
        }

        internal async Task<ServiceResultDto<string>> Login(LoginRequest request)
        {

            var user = await _appDbContext.Usuarios.SingleOrDefaultAsync(u => u.Login == request.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash)) return ValidationError<string>("Credenciais Incorretas.");

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
            return Ok(jwt);

        }
    }
}