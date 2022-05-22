using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Game
    {
        public enum SeasonEnum { Winter = 0, Summer = 1 }

        public int Id { get; set; }

        public int Year { get; set; }
        public SeasonEnum Season { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

        public object Short() => new
        {
            Id,
            Year,
            Season = Season.ToString() + ": " + (int)Season,
            Country = Country?.Short()
        };

        public object Long() => new
        {
            Id,
            Year,
            Season = Season.ToString() + ": " + (int)Season,
            Country = Country?.Short(),
            Participations = Participations.Select(p => p.Short())
        };
    }
}
