using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OlympicGamesAPIWebApp.Models
{
    public class Sport
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Event> Events { get; set; } = new HashSet<Event>();

        public object Short() => new
        {
            Id,
            Name
        };

        public object Long() => new
        {
            Id,
            Name,
            Events = Events.Select(p => p.Short())
        };
    }
}
