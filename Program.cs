using GamesList.Databases;
using GamesList.Models;
using GamesList.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddControllers();
builder.Services.AddScoped<GameService>();
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
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameDB>();

    context.Games.Add(new Game { Id = 1, GameName = "Disco Elysium", Genres = ["RPG Isométrico", "Mistério", "Investigação", "Ficção Científica"], Rating = 10, Opinion = "Definitivamente, o melhor jogo que já joguei. Não canso de tecer elogios a profundidade e carinho que esse jogo possui. Trata assuntos íntimos e externos com sessões de delicadeza e deboche de forma que apenas algo muito bem trabalhado poderia fazer." });
    context.Games.Add(new Game { Id = 2, GameName = "The Binding Of Isaac: Rebirth", Genres = ["Roguelite", "Ação e aventura"], Rating = 9, Opinion = "Depois de mais de 450 horas de jogo, apenas posso dizer que é um ótimo jogo de custo-benefício. Roguelite extremamente complexo com bizarrices que escondem temas pesados de trauma e abuso infantil." });
    context.Games.Add(new Game { Id = 3, GameName = "Engima do Medo", Genres = ["Mistério", "Terror", "Sobrenatural", "Investigação"], Rating = 9, Opinion = "Ainda não terminei, mas tenho gostado bastante do desenrolar desse jogo. Apesar de não ser o maior consumidor de Ordem Paranormal, posso dizer que estou conseguindo apreciar muito bem o jogo. Se tenho que adicionar um defeito, eu diria que fiquei infeliz com o sistema de combate." });
    context.SaveChanges();
}
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
