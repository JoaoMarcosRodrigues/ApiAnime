using ApiAnime.Context;
using ApiAnime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly AnimeContext _animeContext;

        public AnimeController(AnimeContext animeContext)
        {
            _animeContext = animeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes()
        {
            if(_animeContext.Animes == null)
            {
                return NotFound();
            }

            return await _animeContext.Animes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Anime>> GetAnimeById(int id)
        {
            if(_animeContext.Animes == null)
            {
                return NotFound();
            }

            var anime = await _animeContext.Animes.FindAsync(id);

            if (anime == null)
            {
                return NotFound();
            }

            return anime;
        }

        [HttpGet("{nome}")]
        public async Task<ActionResult<Anime>> GetAnimeByNome(string nome)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var anime = await _animeContext.Animes.FindAsync(nome);

            if(anime == null)
            {
                return NotFound();
            }

            return anime;
        }

        [HttpGet("{diretor}")]
        public async Task<ActionResult<Anime>> GetAnimeByDiretor(string diretor)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var anime = await _animeContext.Animes.FindAsync(diretor);

            if (anime == null)
            {
                return NotFound();
            }

            return anime;
        }

        [HttpGet("{palavra_chave}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimeByPalavraChaveResumo(string palavra_chave)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.Where(x => x.RESUMO.Contains(palavra_chave)).ToListAsync();

            if (animes == null)
            {
                return NotFound();
            }

            return animes;
        }

        [HttpPost]
        public async Task<ActionResult<Anime>> CadastroAnime(Anime anime)
        {
            _animeContext.Animes.Add(anime);

            await _animeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Anime), new {ID = anime.ID}, anime);
        }
    }
}
