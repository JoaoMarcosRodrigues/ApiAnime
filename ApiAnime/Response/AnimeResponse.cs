using ApiAnime.Models;

namespace ApiAnime.Response
{
    public class AnimeResponse
    {
        public List<Anime> Animes { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
