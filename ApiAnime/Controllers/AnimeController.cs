using ApiAnime.Context;
using ApiAnime.Models;
using ApiAnime.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int page, int pageSize)
        {
            if(_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.ToListAsync();

            var totalCount = animes.Count;
            var pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var animesPerPage = animes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            _logger.LogInformation("Listagem de todos os animes: \n"+ JsonConvert.SerializeObject(animes));
            _logger.LogInformation("Listagem de todos os animes da página "+page+ "\n" + JsonConvert.SerializeObject(animesPerPage));

            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return Ok(response);
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

            _logger.LogInformation("Listagem de animes por Id: \n" + JsonConvert.SerializeObject(anime));

            return anime;
        }

        [Authorize]
        [HttpGet("porNome/{nome}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimeByNome(string nome, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.Where(x => x.NOME != null && x.NOME.ToLower() == nome.ToLower()).ToListAsync();

            var totalCount = animes.Count;
            var pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var animesPerPage = animes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Nome: \n" + JsonConvert.SerializeObject(animes));
            _logger.LogInformation("Listagem de animes por Nome da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage));

            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("porDiretor/{diretor}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimeByDiretor(string diretor, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.Where(x => x.DIRETOR != null && x.DIRETOR.ToLower() == diretor.ToLower()).ToListAsync();

            var totalCount = animes.Count;
            var pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var animesPerPage = animes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Diretor: \n" + JsonConvert.SerializeObject(animes));
            _logger.LogInformation("Listagem de animes por Diretor da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage));

            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("porPalavraChave/{palavra_chave}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimeByPalavraChaveResumo(string palavra_chave, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            var animes = await _animeContext.Animes.Where(x => x.RESUMO != null && x.RESUMO.ToLower().Contains(palavra_chave.ToLower())).ToListAsync();

            var totalCount = animes.Count;
            var pageCount = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var animesPerPage = animes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (animes == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Listagem de animes por Palavras Chaves no Resumo: \n" + JsonConvert.SerializeObject(animes));
            _logger.LogInformation("Listagem de animes por Palavras Chaves no Resumo da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage));

            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return Ok(response);
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
