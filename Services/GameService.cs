using GamesList.Databases;
using GamesList.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Services
{
    public class GameService(GameDB gameDB)
    {
        private GameDB _gameDB = gameDB;

        public async Task<List<Game>> GetAllGames()
        {
            return await _gameDB.Games.ToListAsync();
        }
        public async Task<Game?> FindGame(int id)
        {
            var result = await _gameDB.Games.FindAsync(id);
            return result;
        }

        public async Task<Game?> UpdateGame(Game game)
        {
            if (game.Id <= 0) return null;
            var existing = await _gameDB.Games.FindAsync(game.Id);
            if (existing == null) return null;

            _gameDB.Entry(existing).CurrentValues.SetValues(game);
            await _gameDB.SaveChangesAsync();
            return existing;
        }


        public async Task<Game?> SaveGame(Game game)
        {
            _gameDB.Games.Add(game);
            await _gameDB.SaveChangesAsync();

            return game;
        }

        public async Task<Game?> DeleteGame(int id)
        {
            if (await _gameDB.Games.FindAsync(id) is Game game)
            {
                _gameDB.Games.Remove(game);
                await _gameDB.SaveChangesAsync();
                return game;
            }
            return null;
        }
    }
}