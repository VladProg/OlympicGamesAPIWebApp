using OlympicGamesAPIWebApp;
using Microsoft.EntityFrameworkCore;
using OlympicGamesAPIWebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OlympicGamesAPIWebApp.Controllers;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

await new UnitTest.UnitTest().Test();

namespace UnitTest
{
    public static class DynamicExtensions
    {
        public static object Get(this object obj, string property)
        {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }
    }

    public class UnitBase
    {
        private readonly WebApplication _app;

        protected UnitBase()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<OlympicGamesAPIContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));
            _app = builder.Build();
            using var scope = Scope();
            var context = Context(scope);
            context.Database.EnsureDeleted();
        }

        protected IServiceScope Scope() => _app.Services.CreateScope();
        protected OlympicGamesAPIContext Context(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<OlympicGamesAPIContext>();

        protected AthletesController Athletes(IServiceScope scope) => new(Context(scope));
        protected CountriesController Countries(IServiceScope scope) => new(Context(scope));
        protected EventsController Events(IServiceScope scope) => new(Context(scope));
        protected GamesController Games(IServiceScope scope) => new(Context(scope));
        protected ParticipationsController Participations(IServiceScope scope) => new(Context(scope));
        protected SportsController Sports(IServiceScope scope) => new(Context(scope));
    }

    public class UnitTest: UnitBase
    {
        private static void CheckCountry(object country, bool changedCountry = false)
        {
            Assert.Equal(changedCountry? "Asdfgh" : "Qwerty", country.Get("Name"));
            Assert.Null(country.Get("FlagUrl"));
        }

        private static void CheckGame(object game, bool changedCountry = false, bool changedGame = false, bool deletedGame = false)
        {
            Assert.Equal(changedGame ? 2 : 1, game.Get("Year"));
            Assert.Equal(changedGame ? "Summer: 1" : "Winter: 0", game.Get("Season"));
            object country = game.Get("Country");
            if (deletedGame)
                Assert.Null(country);
            else
                CheckCountry(country, changedCountry: changedCountry);
        }

        [Fact]
        public async Task Test()
        {
            int countryId;

            using(var scope=Scope())
            {
                object country = ((await Countries(scope).PostCountry(new() { Name = "Qwerty" })).Result as CreatedAtActionResult)?.Value;
                CheckCountry(country);
                IEnumerable<object> games = country.Get("Games") as IEnumerable<object>;
                Assert.Empty(games);
                countryId = (int)country.Get("Id");
            }

            using (var scope = Scope())
            {
                object country = (await Countries(scope).GetCountry(countryId)).Value;
                CheckCountry(country);
                IEnumerable<object> games = country.Get("Games") as IEnumerable<object>;
                Assert.Empty(games);
            }

            using (var scope = Scope())
            {
                IEnumerable<object> countries = (await Countries(scope).GetCountries()).Value;
                object country = Assert.Single(countries);
                CheckCountry(country);
            }

            int gameId;

            using (var scope = Scope())
            {
                object game = ((await Games(scope).PostGame(new() { Year = 1, Season = Game.SeasonEnum.Winter, CountryId = countryId })).Result as CreatedAtActionResult)?.Value;
                CheckGame(game);
                gameId = (int)game.Get("Id");
            }

            using (var scope = Scope())
            {
                object game = (await Games(scope).GetGame(gameId)).Value;
                CheckGame(game);
            }

            using (var scope = Scope())
            {
                IEnumerable<object> games = (await Games(scope).GetGames()).Value;
                object game = Assert.Single(games);
                CheckGame(game);
            }

            using (var scope = Scope())
            {
                object country = (await Countries(scope).GetCountry(countryId)).Value;
                CheckCountry(country);
                IEnumerable<object> games = country.Get("Games") as IEnumerable<object>;
                object game = Assert.Single(games);
                CheckGame(game);
            }

            using (var scope = Scope())
            {
                object country = ((await Countries(scope).PutCountry(countryId, new() { Id = countryId, Name = "Asdfgh" })).Result as CreatedAtActionResult)?.Value;
                CheckCountry(country, changedCountry: true);
                IEnumerable<object> games = country.Get("Games") as IEnumerable<object>;
                object game = Assert.Single(games);
                CheckGame(game, changedCountry: true);
            }

            using (var scope = Scope())
            {
                object game = ((await Games(scope).PutGame(gameId, new() { Id = gameId, Year = 2, Season = Game.SeasonEnum.Summer, CountryId = countryId })).Result as CreatedAtActionResult)?.Value;
                CheckGame(game, changedCountry: true, changedGame: true);
            }

            using (var scope = Scope())
            {
                await Assert.ThrowsAnyAsync<Exception>(() => Countries(scope).DeleteCountry(countryId));
            }
        }
    }
}