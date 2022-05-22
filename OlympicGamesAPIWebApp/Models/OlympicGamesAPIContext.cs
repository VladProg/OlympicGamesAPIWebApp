using Microsoft.EntityFrameworkCore;

namespace OlympicGamesAPIWebApp.Models
{
    public class OlympicGamesAPIContext: DbContext
    {
        public virtual DbSet<Athlete> Athletes { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Participation> Participations { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }

        public OlympicGamesAPIContext(DbContextOptions<OlympicGamesAPIContext> options): base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
