using System.Text;
using GamesList.Databases;
using GamesList.Repositories.AuthRepository;
using GamesList.Repositories.AvaliacaoRepository;
using GamesList.Repositories.GeneroRepository;
using GamesList.Repositories.ImagensRepository;
using GamesList.Repositories.SugestoesImagemRepository;
using GamesList.Repositories.JogoRepository;
using GamesList.Repositories.SugestoesJogoRepository;
using GamesList.Repositories.UnitOfWork;
using GamesList.Services.AuthService;
using GamesList.Services.AvaliacaoService;
using GamesList.Services.BlobService;
using GamesList.Services.GeneroService;
using GamesList.Services.ImagensService;
using GamesList.Services.JogoService;
using GamesList.Services.SugestoesJogoService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GamesList.Services.SugestoesImagemService;
using GamesList.Repositories.RoleRepository;
using EFCore.NamingConventions;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var provider = Environment.GetEnvironmentVariable("DB_PROVIDER")?.ToLower();
string? connectionString = provider switch
{
    "postgresql" => Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING"),
    "azure" => Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING"),
    _ => throw new Exception("DB_PROVIDER inválido")

};
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
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    switch (provider)
    {
        case "postgresql":
            opt.UseNpgsql(connectionString, o => o.UseRelationalNulls()).UseSnakeCaseNamingConvention();
            break;
        case "azure":
            opt.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null));
            break;
        default:
            throw new Exception("DB_PROVIDER inválido");

    } }
);
builder.Services.AddControllers();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IImagensRepository, ImagensRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<ISugestoesJogoRepository, SugestoesJogoRepository>();
builder.Services.AddScoped<ISugestoesImagemRepository, SugestoesImagemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IImagensService, ImagensServices>();
builder.Services.AddScoped<IBlobService, AzureBlobService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>(); 
builder.Services.AddScoped(provider => new Lazy<IAvaliacaoService>(() =>
    provider.GetRequiredService<IAvaliacaoService>()));
builder.Services.AddScoped<ISugestoesJogoService, SugestoesJogoService>();
builder.Services.AddScoped<ISugestoesImagemService, SugestoesImagemService>();

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
