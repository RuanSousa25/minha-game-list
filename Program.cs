using System.Text;
using GamesList.Databases;
using GamesList.Models;
using GamesList.Services;
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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JogoService>();
builder.Services.AddScoped<ImagensSugestaoService>();
builder.Services.AddScoped<SugerirJogoService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<AvaliacaoService>();
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
