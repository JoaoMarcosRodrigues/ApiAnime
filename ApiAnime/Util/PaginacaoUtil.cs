using ApiAnime.Models;

namespace ApiAnime.Util
{
    public class PaginacaoUtil
    {
        public static void GetPaginacao(int page, int pageSize, List<Anime> animes, out int pageCount, out List<Anime> animesPerPage)
        {
            var totalCount = animes.Count;
            pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);
            animesPerPage = animes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
