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
            // Configura��o do DbContextOptions para um banco de dados em mem�ria
            _options = new DbContextOptionsBuilder<AnimeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // M�todo para criar um DbContext tempor�rio para cada teste
        private AnimeContext CreateDbContext()
        {
            return new AnimeContext(_options);
        }

        private void Dispose()
        {
            // Limpeza do banco de dados em mem�ria ap�s os testes
            using (var context = CreateDbContext())
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact(DisplayName = "Insira um ID v�lido para obter o anime espec�fico.")]
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
                    RESUMO = "bl�bl�bl�"
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

        [Fact(DisplayName = "Insira um Nome v�lido para obter o anime espec�fico.")]
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
                    RESUMO = "bl�bl�bl�"
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

        [Fact(DisplayName = "Insira um Diretor v�lido para obter o anime espec�fico.")]
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
                    RESUMO = "bl�bl�bl�"
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

        [Fact(DisplayName = "Insira uma Palavra-Chave v�lida para obter o anime espec�fico.")]
        public async void GivenValidPalavraChave_WhenGetAnimeByPalavraChaveResumo_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, _logger);
                const string palavra_chave = "bl�";
                Anime animeEsperado = new Anime()
                {
                    ID = 1,
                    NOME = "Fairy Tail",
                    DIRETOR = "Natsu",
                    RESUMO = "bl�bl�bl�"
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