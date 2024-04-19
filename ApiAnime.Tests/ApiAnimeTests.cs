using ApiAnime.Context;
using ApiAnime.Controllers;
using ApiAnime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiAnime.Tests
{
    public class ApiAnimeTests
    {
        private readonly AnimeContext _animeContext;

        public ApiAnimeTests(AnimeContext animeContext)
        {
            _animeContext = animeContext;
        }

        [Fact(DisplayName = "Insira um ID v�lido para obter o anime espec�fico.")]
        public async void GivenValidID_WhenGetAnimeById_ThenShouldSuccess()
        {
            // Arrange
            const int id = 1;
            Anime animeEsperado = new Anime()
            {
                ID = id,
                NOME = "Fairy Tail",
                DIRETOR = "Natsu",
                RESUMO = "bl�bl�bl�"
            };

            // Act
            var anime = await _animeContext.Animes.FindAsync(id);

            // Assert
            Assert.Equal(animeEsperado,anime);
        }

        [Fact(DisplayName = "Insira um Nome v�lido para obter o anime espec�fico.")]
        public async void GivenValidNome_WhenGetAnimeByNome_ThenShouldSuccess()
        {
            // Arrange
            const string nome = "Fairy Tail";
            Anime animeEsperado = new Anime()
            {
                ID = 1,
                NOME = nome,
                DIRETOR = "Natsu",
                RESUMO = "bl�bl�bl�"
            };

            // Act
            var anime = await _animeContext.Animes.FindAsync(nome);

            // Assert
            Assert.Equal(animeEsperado, anime);
        }

        [Fact(DisplayName = "Insira um Diretor v�lido para obter o anime espec�fico.")]
        public async void GivenValidDiretor_WhenGetAnimeByDiretor_ThenShouldSuccess()
        {
            // Arrange
            const string diretor = "Natsu";
            Anime animeEsperado = new Anime()
            {
                ID = 1,
                NOME = "Fairy Tail",
                DIRETOR = diretor,
                RESUMO = "bl�bl�bl�"
            };

            // Act
            var anime = await _animeContext.Animes.FindAsync(diretor);

            // Assert
            Assert.Equal(animeEsperado, anime);
        }

        [Fact(DisplayName = "Insira uma Palavra-Chave v�lida para obter o anime espec�fico.")]
        public async void GivenValidPalavraChave_WhenGetAnimeByPalavraChaveResumo_ThenShouldSuccess()
        {
            // Arrange
            const string palavra_chave = "bl�";

            Anime anime = new Anime()
            {
                ID = 1,
                NOME = "Fairy Tail",
                DIRETOR = "Natsu",
                RESUMO = "bl�bl�bl�"
            };

            List<Anime> animesEsperados = new List<Anime>();
            animesEsperados.Add(anime);

            // Act
            var animes = await _animeContext.Animes.Where(x => x.RESUMO != null && x.RESUMO.Contains(palavra_chave)).Distinct().ToListAsync();

            // Assert
            Assert.Equal(animesEsperados, animes);
        }
    }
}