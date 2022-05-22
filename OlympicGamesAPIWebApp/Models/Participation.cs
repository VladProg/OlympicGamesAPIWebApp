using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Participation
    {
        public enum MedalEnum { No = 0, Gold = 1, Silver = 2, Bronze = 3 }

        public int Id { get; set; }

        public MedalEnum Medal { get; set; }

        public int AthleteId { get; set; }
        public int CountryId { get; set; }
        public int EventId { get; set; }
        public int GameId { get; set; }

        public Athlete Athlete { get; set; }
        public Country Country { get; set; }
        public Event Event { get; set; }
        public Game Game { get; set; }

        public Participation SetNull()
        {
            Athlete.Participations = null;
            Country.Participations = null;
            Event.Participations = null;
            Game.Participations = null;
            return this;
        }

        public object Short() => new
        {
            Id,
            Medal = Medal.ToString() + ": " + (int)Medal,
            Athlete = Athlete?.Short(),
            Country = Country?.Short(),
            Event = Event?.Short(),
            Game = Game?.Short()
        };

        public object Long() => new
        {
            Id,
            Medal = Medal.ToString() + ": " + (int)Medal,
            Athlete = Athlete?.Short(),
            Country = Country?.Short(),
            Event = Event?.Short(),
            Game = Game?.Short()
        };
    }
}
