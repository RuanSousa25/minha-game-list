using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using Microsoft.IdentityModel.Tokens;

namespace GamesList.Services
{
    public class AuthService(AppDbContext appDbContext, IConfiguration config)
    {
        private AppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _config = config;

        public async Task<ServiceResultDto<string>> Register(RegisterRequest request)
        {
            if (_appDbContext.Usuarios.Any(u => u.Login == request.Login)) return ServiceResultDto<string>.Fail("Usuário já existe");
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var user = new Usuario { Login = request.Login, SenhaHash = hash };

            _appDbContext.Usuarios.Add(user);
            _appDbContext.SaveChanges();

            return ServiceResultDto<string>.Ok("Usuario Cadastrado");
        }

        internal async Task<ServiceResultDto<string>> Login(LoginRequest request)
        {

            var user = _appDbContext.Usuarios.SingleOrDefault(u => u.Login == request.Login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash)) return ServiceResultDto<string>.Fail("Credenciais Incorretas.");

            var claims = new[]{
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user")
            };
            var jwtSecret = _config["JWT_SECRET"];
            if (jwtSecret == null) return ServiceResultDto<string>.Fail("Serviço indisponível.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return ServiceResultDto<string>.Ok(jwt);

        }
    }
}