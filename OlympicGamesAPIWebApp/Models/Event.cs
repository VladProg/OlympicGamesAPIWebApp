using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int SportId { get; set; }

        public Sport Sport { get; set; }

        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

        public object Short() => new
        {
            Id,
            Name,
            Sport = Sport?.Short()
        };

        public object Long() => new
        {
            Id,
            Name,
            Sport = Sport.Short(),
            Participations = Participations.Select(p => p.Short())
        };
    }
}
