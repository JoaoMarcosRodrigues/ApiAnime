using ApiAnime.Context;
using ApiAnime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly AnimeContext _animeContext;
        private readonly ILogger<AnimeController> _logger;

        public AnimeController(AnimeContext animeContext, ILogger<AnimeController> logger)
        {
            _animeContext = animeContext;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes()
        {
            if(_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.ToListAsync();

            _logger.LogInformation("Listagem de todos os animes: \n"+ animes);

            return animes;
        }

        [Authorize]
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

            _logger.LogInformation("Listagem de animes por Id: \n" + anime);

            return anime;
        }

        [Authorize]
        [HttpGet("porNome/{nome}")]
        public async Task<ActionResult<Anime>> GetAnimeByNome(string nome)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.FindAsync(nome);

            if(animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Nome: \n" + animes);

            return animes;
        }

        [Authorize]
        [HttpGet("porDiretor/{diretor}")]
        public async Task<ActionResult<Anime>> GetAnimeByDiretor(string diretor)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.FindAsync(diretor);

            if (animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Diretor: \n" + animes);

            return animes;
        }

        [Authorize]
        [HttpGet("porPalavraChave/{palavra_chave}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimeByPalavraChaveResumo(string palavra_chave)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.Where(x => x.RESUMO != null && x.RESUMO.Contains(palavra_chave)).ToListAsync();

            if (animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Palavras Chaves no Resumo: \n" + animes);

            return animes;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Anime>> CadastroAnime(Anime anime)
        {
            _animeContext.Animes.Add(anime);

            await _animeContext.SaveChangesAsync();

            CreatedAtActionResult resultado = CreatedAtAction(nameof(GetAnimeById), new { id = anime.ID }, anime);

            _logger.LogInformation("Anime cadastrado com sucesso!");

            return resultado;
        }

        [Authorize]
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

            _logger.LogInformation("Anime editado com sucesso!");

            return Ok();
        }

        [Authorize]
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

            _logger.LogInformation("Anime excluído com sucesso!");

            return Ok();
        }
    }
}
