using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string FlagUrl { get; set; }

        public ICollection<Game> Games { get; set; } = new HashSet<Game>();
        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

        public object Short() => new
        {
            Id,
            Name,
            FlagUrl
        };

        public object Long() => new
        {
            Id,
            Name,
            FlagUrl,
            Games = Games.Select(g => g.Short()),
            Participations = Participations.Select(g => g.Short())
        };
    }
}
