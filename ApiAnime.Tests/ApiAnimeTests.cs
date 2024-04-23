using ApiAnime.Context;
using ApiAnime.Controllers;
using ApiAnime.Models;
using ApiAnime.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAnime.Tests
{
    public class ApiAnimeTests
    {
        private readonly DbContextOptions<AnimeContext> _options;

        public ApiAnimeTests()
        {
            // Configuração do DbContextOptions para um banco de dados em memória
            _options = new DbContextOptionsBuilder<AnimeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
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

        [Fact(DisplayName = "Insira um ID válido para obter o Anime específico.")]
        public async void GivenValidID_WhenGetAnimeById_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, null);
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
                ActionResult<Anime> resultado = await animeController.GetAnimeById(id);
                Anime? anime = (Anime?)resultado.Value;

                // Assert
                Assert.NotNull(anime);
                Assert.Equal(animeEsperado.ID, anime.ID);
                Assert.Equal(animeEsperado.NOME, anime.NOME);
                Assert.Equal(animeEsperado.DIRETOR, anime.DIRETOR);
                Assert.Equal(animeEsperado.RESUMO, anime.RESUMO);
            }
            Dispose();
        }

        [Fact(DisplayName = "Insira um Nome válido para obter Animes específicos.")]
        public async void GivenValidNome_WhenGetAnimeByNome_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, null);
                const string nome = "Fairy Tail";
                Anime animeEsperado1 = new Anime()
                {
                    ID = 1,
                    NOME = nome,
                    DIRETOR = "Natsu",
                    RESUMO = "blábláblá"
                };
                Anime animeEsperado2 = new Anime()
                {
                    ID = 2,
                    NOME = nome + " 2",
                    DIRETOR = "Gray",
                    RESUMO = "blebleble"
                };

                context.Animes.Add(animeEsperado1);
                context.Animes.Add(animeEsperado2);
                context.SaveChanges();

                // Act
                ActionResult<AnimeResponse> resultado = await animeController.GetAnimeByNome(nome,1,5);
                List<Anime>? enumerableAnimes = resultado.Value?.Animes;

                // Assert
                Assert.NotNull(enumerableAnimes);
                Assert.Single(enumerableAnimes);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira um Diretor válido para obter Animes específicos.")]
        public async void GivenValidDiretor_WhenGetAnimeByDiretor_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, null);
                const string diretor = "Natsu";
                Anime animeEsperado1 = new Anime()
                {
                    ID = 1,
                    NOME = "Fairy Tail",
                    DIRETOR = diretor,
                    RESUMO = "blábláblá"
                };
                Anime animeEsperado2 = new Anime()
                {
                    ID = 2,
                    NOME = "Fairy Tail 2",
                    DIRETOR = diretor + " e Gray",
                    RESUMO = "blebleble"
                };

                context.Animes.Add(animeEsperado1);
                context.Animes.Add(animeEsperado2);
                context.SaveChanges();

                // Act
                ActionResult<AnimeResponse> resultado = await animeController.GetAnimeByDiretor(diretor, 1, 5);
                List<Anime>? enumerableAnimes = resultado.Value?.Animes;

                // Assert
                Assert.NotNull(enumerableAnimes);
                Assert.Single(enumerableAnimes);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira uma Palavra-Chave válida para obter Animes específicos.")]
        public async void GivenValidPalavraChave_WhenGetAnimeByPalavraChaveResumo_ThenShouldSuccess()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context, null);
                const string palavra_chave = "teste";
                Anime animeEsperado1 = new Anime()
                {
                    ID = 1,
                    NOME = "Fairy Tail",
                    DIRETOR = "Natsu",
                    RESUMO = "teste"
                };
                Anime animeEsperado2 = new Anime()
                {
                    ID = 2,
                    NOME = "Fairy Tail",
                    DIRETOR = "Natsu",
                    RESUMO = "teste"
                };

                context.Animes.Add(animeEsperado1);
                context.Animes.Add(animeEsperado2);
                context.SaveChanges();

                // Act
                ActionResult<AnimeResponse> resultado = await animeController.GetAnimeByPalavraChaveResumo(palavra_chave, 1, 5);
                List<Anime>? enumerableAnimes = resultado.Value?.Animes;

                // Assert
                Assert.NotNull(enumerableAnimes);
                Assert.Equal(2, enumerableAnimes.Count());
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira um Anime para realizar seu cadastro.")]
        public async Task CadastrarAnime_DeveRetornarTrueQuandoCadastradoComSucesso()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var animeController = new AnimeController(context,null);

                // Act
                var resultado = await animeController.CadastroAnime(new Anime { ID = 1, NOME = "Death Note", DIRETOR = "L", RESUMO = "teste" });

                // Assert
                var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
                Assert.Equal(201, createdResult.StatusCode);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira um ID válido e um Anime para editar os dados de um Anime específico.")]
        public async Task EditarAnime_DeveRetornarTrueQuandoEditadoComSucesso()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                // Adicionar um anime inicial ao banco de dados para editar posteriormente
                context.Animes.Add(new Anime { ID = 1, NOME = "Death Note", DIRETOR = "L", RESUMO = "teste" });
                context.SaveChanges();

                var animeController = new AnimeController(context, null);

                // Act
                var novoAnime = new Anime { ID = 1, NOME = "Death Note editado", DIRETOR = "L", RESUMO = "teste" };
                var resultado = await animeController.EditarAnime(1, novoAnime);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(resultado);
                var animeEditado = Assert.IsType<Anime>(okResult.Value);
                Assert.Equal(novoAnime.ID, animeEditado.ID);
                Assert.Equal(novoAnime.NOME, animeEditado.NOME);
                Assert.Equal(novoAnime.DIRETOR, animeEditado.DIRETOR);
                Assert.Equal(novoAnime.RESUMO, animeEditado.RESUMO);
            }

            Dispose();
        }

        [Fact(DisplayName = "Insira um ID válido para excluir um Anime específico.")]
        public async Task ExcluirAnime_DeveRetornarNoContentQuandoExcluidoComSucesso()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                // Adicionar um anime inicial ao banco de dados para excluir posteriormente
                context.Animes.Add(new Anime { ID = 1, NOME = "Death Note", DIRETOR = "L", RESUMO = "teste" });
                context.SaveChanges();

                var animeController = new AnimeController(context, null);

                // Act
                var resultado = await animeController.DeletarAnime(1);

                // Assert
                Assert.IsType<OkResult>(resultado);
            }

            Dispose();
        }
    }   
}