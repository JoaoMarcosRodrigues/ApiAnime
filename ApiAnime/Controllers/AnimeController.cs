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

        [HttpGet("porId/{id}")]
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

        [HttpGet("porNome/{nome}")]
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

        [HttpGet("porDiretor/{diretor}")]
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

        [HttpGet("porPalavraChave/{palavra_chave}")]
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

            return CreatedAtAction(nameof(GetAnimeById), new {id = anime.ID}, anime);
        }

        [HttpPut]
        public async Task<IActionResult> EditarAnime(int id, Anime anime)
        {
            if(id != anime.ID)
            {
                return BadRequest();
            }

            _animeContext.Entry(anime).State = EntityState.Modified;

            try
            {
                await _animeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAnime(int id)
        {
            if(_animeContext == null)
            {
                return NotFound();
            }

            var anime = await _animeContext.Animes.FindAsync(id);

            if(anime == null)
            {
                return NotFound();
            }

            _animeContext.Animes.Remove(anime);

            await _animeContext.SaveChangesAsync();

            return Ok();
        }
    }
}
