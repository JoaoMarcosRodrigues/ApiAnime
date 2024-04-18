using ApiAnime.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAnime.Context
{
    public class AnimeContext : DbContext
    {
        public AnimeContext(DbContextOptions<AnimeContext> options) : base(options)
        {
            
        }

        public DbSet<Anime> Animes { get; set; }
    }
}
