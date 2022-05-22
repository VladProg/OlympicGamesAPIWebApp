using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Athlete
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

        public Athlete SetNull()
        {
            foreach (var p in Participations)
                p.Athlete = null;
            return this;
        }

        public object Short() => new
        {
            Id,
            Name
        };

        public object Long() => new
        {
            Id,
            Name,
            Participations = Participations.Select(p => p.Short())
        };
    }
}
