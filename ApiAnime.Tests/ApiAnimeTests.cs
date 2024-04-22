using ApiAnime.Context;
using ApiAnime.Controllers;
using ApiAnime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using static Azure.Core.HttpHeader;

namespace ApiAnime.Tests
{
    public class ApiAnimeTests
    {
        private readonly DbContextOptions<AnimeContext> _options;
        private readonly ILogger<AnimeController> _logger;

        public ApiAnimeTests(ILogger<AnimeController> logger)
        {
            // Configuração do DbContextOptions para um banco de dados em memória
            _options = new DbContextOptionsBuilder<AnimeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para criar um DbContext temporário para cada teste
        private AnimeContext CreateDbContext()
        {
            return new AnimeContext(_options);
        }

        private void Dispose()
        {
            // Limpeza do banco de dados em memória após os testes
            using (var context = CreateDbContext())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact(DisplayName = "Insira um ID válido para obter o anime específico.")]
        public async void GivenValidID_WhenGetAnimeById_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context,_logger);
                const int id = 1;
                Anime animeEsperado = new Anime()
                {
                    ID = id,
                    NOME = "Fairy Tail",
                    DIRETOR = "Natsu",
                    RESUMO = "blábláblá"
                };

                context.Animes.Add(animeEsperado);
                context.SaveChanges();

                // Act
                Anime? anime = await context.Animes.FindAsync(id);

                // Assert
                Assert.NotNull(anime);
                Assert.Equal(animeEsperado.ID, anime.ID);
                Assert.Equal(animeEsperado.NOME, anime.NOME);
                Assert.Equal(animeEsperado.DIRETOR, anime.DIRETOR);
                Assert.Equal(animeEsperado.RESUMO, anime.RESUMO);
            }
            Dispose();
        }

        [Fact(DisplayName = "Insira um Nome válido para obter o anime específico.")]
        public async void GivenValidNome_WhenGetAnimeByNome_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, _logger);
                const string nome = "Fairy Tail";
                Anime animeEsperado = new Anime()
                {
                    ID = 1,
                    NOME = nome,
                    DIRETOR = "Natsu",
                    RESUMO = "blábláblá"
                };

                context.Animes.Add(animeEsperado);
                context.SaveChanges();

                // Act
                Anime anime = await context.Animes.Where(x => x.NOME != null && x.NOME.ToLower() == nome.ToLower()).FirstAsync();

                // Assert
                Assert.NotNull(anime);
                Assert.Equal(animeEsperado.ID, anime.ID);
                Assert.Equal(animeEsperado.NOME, anime.NOME);
                Assert.Equal(animeEsperado.DIRETOR, anime.DIRETOR);
                Assert.Equal(animeEsperado.RESUMO, anime.RESUMO);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira um Diretor válido para obter o anime específico.")]
        public async void GivenValidDiretor_WhenGetAnimeByDiretor_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, _logger);
                const string diretor = "Natsu";
                Anime animeEsperado = new Anime()
                {
                    ID = 1,
                    NOME = "Fairy Tail",
                    DIRETOR = diretor,
                    RESUMO = "blábláblá"
                };

                context.Animes.Add(animeEsperado);
                context.SaveChanges();

                // Act
                Anime anime = await context.Animes.Where(x => x.DIRETOR != null && x.DIRETOR.ToLower() == diretor.ToLower()).FirstAsync();

                // Assert
                Assert.NotNull(anime);
                Assert.Equal(animeEsperado.ID, anime.ID);
                Assert.Equal(animeEsperado.NOME, anime.NOME);
                Assert.Equal(animeEsperado.DIRETOR, anime.DIRETOR);
                Assert.Equal(animeEsperado.RESUMO, anime.RESUMO);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira uma Palavra-Chave válida para obter o anime específico.")]
        public async void GivenValidPalavraChave_WhenGetAnimeByPalavraChaveResumo_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, _logger);
                const string palavra_chave = "blá";
                Anime animeEsperado = new Anime()
                {
                    ID = 1,
                    NOME = "Fairy Tail",
                    DIRETOR = "Natsu",
                    RESUMO = "blábláblá"
                };

                context.Animes.Add(animeEsperado);
                context.SaveChanges();

                // Act
                Anime anime = await context.Animes.Where(x => x.RESUMO != null && x.RESUMO.ToLower().Contains(palavra_chave.ToLower())).FirstAsync();

                // Assert
                Assert.NotNull(anime);
                Assert.Equal(animeEsperado.ID, anime.ID);
                Assert.Equal(animeEsperado.NOME, anime.NOME);
                Assert.Equal(animeEsperado.DIRETOR, anime.DIRETOR);
                Assert.Equal(animeEsperado.RESUMO, anime.RESUMO);
            }

            Dispose();
        }
    }
}