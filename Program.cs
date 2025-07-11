using System.Text;
using GamesList.Databases;
using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GenerosRepository;
using GamesList.Repositories.ImagensRepository;
using GamesList.Repositories.ImagensSugestaoRepository;
using GamesList.Repositories.JogoRepository;
using GamesList.Repositories.SugerirJogoRepository;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AuthService;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.BlobService;
using GamesList.Services.ImagensService;
using GamesList.Services.ImagensSugestaoService;
using GamesList.Services.JogoService;
using GamesList.Services.SugerirJogoService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_DEFAULT");
var key = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(key)){ return; }

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure( maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)));
builder.Services.AddControllers();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IGenerosRepository, GenerosRepository>();
builder.Services.AddScoped<IImagensRepository, ImagensRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<ISugerirJogoRepository, SugerirJogoRepository>();
builder.Services.AddScoped<IImagensSugestaoRepository, ImagensSugestaoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IImagensService, ImagensServices>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>(); 
builder.Services.AddScoped<ISugerirJogoService, SugerirJogoService>();
builder.Services.AddScoped<IImagensSugestaoService, ImagensSugestaoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
