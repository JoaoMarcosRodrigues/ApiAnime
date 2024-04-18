namespace ApiAnime.Models
{
    public class Anime
    {
        public int ID { get; set; }
        public required string NOME { get; set; }
        public required string RESUMO { get; set; }
        public required string DIRETOR { get; set; }
    }
}
