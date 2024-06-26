﻿using ApiAnime.Context;
using ApiAnime.Models;
using ApiAnime.Response;
using ApiAnime.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly AnimeContext _animeContext;
        private readonly ILogger<AnimeController>? _logger;

        public AnimeController(AnimeContext animeContext, ILogger<AnimeController>? logger)
        {
            _animeContext = animeContext;
            _logger = logger;
        }

        // Obter todos os Animes cadastrados
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            // Consulta
            var animes = await _animeContext.Animes.ToListAsync();

            // Paginação
            int pageCount;
            List<Anime> animesPerPage;
            PaginacaoUtil.GetPaginacao(page, pageSize, animes, out pageCount, out animesPerPage);

            // Logs
            if (_logger != null)
            {
                _logger.LogInformation("\nListagem de todos os animes: \n" + JsonConvert.SerializeObject(animes) + "\n");
                _logger.LogInformation("\nListagem de todos os animes da página " + page + "\n" + JsonConvert.SerializeObject(animesPerPage) + "\n");
            }

            // Montagem resposta
            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return Ok(response);
        }

        // Obter Anime por ID
        [Authorize]
        [HttpGet("porId/{id}")]
        public async Task<ActionResult<Anime>> GetAnimeById(int id)
        {
            if(_animeContext.Animes == null)
            {
                return NotFound();
            }

            // Consulta
            var anime = await _animeContext.Animes.FindAsync(id);

            if (anime == null)
            {
                return NotFound();
            }

            // Log
            if (_logger != null)
                _logger.LogInformation("\nListagem de animes por Id: \n" + JsonConvert.SerializeObject(anime) + "\n");

            return anime;
        }

        // Obter Anime por Nome
        [Authorize]
        [HttpGet("porNome/{nome}")]
        public async Task<ActionResult<AnimeResponse>> GetAnimeByNome(string nome, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            // Consulta
            var animes = await _animeContext.Animes.Where(x => x.NOME != null && x.NOME.ToLower() == nome.ToLower()).ToListAsync();

            // Paginação
            int pageCount;
            List<Anime> animesPerPage;
            PaginacaoUtil.GetPaginacao(page, pageSize, animes, out pageCount, out animesPerPage);

            if (animes == null)
            {
                return NotFound();
            }

            // Logs
            if (_logger != null)
            {
                _logger.LogInformation("\nListagem de animes por nome: \n" + JsonConvert.SerializeObject(animes) + "\n");
                _logger.LogInformation("\nListagem de animes por nome da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage) + "\n");
            }

            // Montagem resposta
            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return response;
        }

        // Obter Anime por Diretor
        [Authorize]
        [HttpGet("porDiretor/{diretor}")]
        public async Task<ActionResult<AnimeResponse>> GetAnimeByDiretor(string diretor, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            // Consulta
            var animes = await _animeContext.Animes.Where(x => x.DIRETOR != null && x.DIRETOR.ToLower() == diretor.ToLower()).ToListAsync();

            // Paginação
            int pageCount;
            List<Anime> animesPerPage;
            PaginacaoUtil.GetPaginacao(page, pageSize, animes, out pageCount, out animesPerPage);

            if (animes == null)
            {
                return NotFound();
            }

            // Logs
            if (_logger != null)
            {
                _logger.LogInformation("\nListagem de animes por Diretor: \n" + JsonConvert.SerializeObject(animes) + "\n");
                _logger.LogInformation("\nListagem de animes por Diretor da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage) + "\n");
            }

            // Montagem resposta
            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return response;
        }

        // Obter Anime que tenha Palavra Chave no Resumo
        [Authorize]
        [HttpGet("porPalavraChave/{palavra_chave}")]
        public async Task<ActionResult<AnimeResponse>> GetAnimeByPalavraChaveResumo(string palavra_chave, int page, int pageSize)
        {
            if (_animeContext.Animes == null)
            {
                return NotFound();
            }

            // Consulta
            var animes = await _animeContext.Animes.Where(x => x.RESUMO != null && x.RESUMO.ToLower().Contains(palavra_chave.ToLower())).ToListAsync();

            // Paginação
            int pageCount;
            List<Anime> animesPerPage;
            PaginacaoUtil.GetPaginacao(page, pageSize, animes, out pageCount, out animesPerPage);

            if (animes == null)
            {
                return NotFound();
            }

            // Logs
            if (_logger != null)
            {
                _logger.LogInformation("\nListagem de animes por Palavras Chaves no Resumo: \n" + JsonConvert.SerializeObject(animes)+"\n");
                _logger.LogInformation("\nListagem de animes por Palavras Chaves no Resumo da página " + page + ":\n" + JsonConvert.SerializeObject(animesPerPage) + "\n");
            }

            // Montagem resposta
            var response = new AnimeResponse
            {
                Animes = animesPerPage,
                Pages = pageCount,
                CurrentPage = page
            };

            return response;
        }

        // Cadastrar Anime
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Anime>> CadastroAnime(Anime anime)
        {
            _animeContext.Animes.Add(anime);

            await _animeContext.SaveChangesAsync();

            CreatedAtActionResult resultado = CreatedAtAction(nameof(GetAnimeById), new { id = anime.ID }, anime);

            if (_logger != null)
                _logger.LogInformation("\nAnime cadastrado com sucesso!\n");

            return resultado;
        }

        // Editar Anime
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditarAnime(int id, Anime anime)
        {
            if(id != anime.ID)
            {
                return BadRequest();
            }

            if(_logger != null)
                _animeContext.Entry(anime).State = EntityState.Modified;

            try
            {
                await _animeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            if (_logger != null)
                _logger.LogInformation("\nAnime editado com sucesso!\n");

            return Ok(anime);
        }

        // Excluir Anime
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

            if (_logger != null)
                _logger.LogInformation("\nAnime excluído com sucesso!\n");

            return Ok();
        }
    }
}
