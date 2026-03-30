using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;
using log660_lab2_serveur.Services;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Services
{
    public class FilmServiceTests
    {
        private readonly Mock<IFilmRepository> _mockFilmRepository;
        private readonly FilmService _filmService;

        public FilmServiceTests()
        {
            _mockFilmRepository = new Mock<IFilmRepository>();
            _filmService = new FilmService(_mockFilmRepository.Object);
        }

        #region GetFilmById Tests

        [Fact]
        public async Task GetFilmById_WithValidId_ReturnsCompleteFilmDto()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(filmId, result.IdFilm);
            Assert.Equal("Inception", result.Titre);
            Assert.Equal(2010, result.Annee);
            Assert.Equal("English", result.Langue);
            Assert.Equal(148, result.Duree);
            Assert.NotNull(result.Realisateur);
            Assert.Equal("Nolan", result.Realisateur.Nom);
            Assert.Equal(2, result.CopiesFilm.Count);
            Assert.Equal(2, result.FilmsGenres.Count());
            Assert.Equal(2, result.FilmsPays.Count());
            Assert.Equal(2, result.Acteurs.Count());
            _mockFilmRepository.Verify(r => r.GetFilmById(filmId), Times.Once);
        }

        [Fact]
        public async Task GetFilmById_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            int filmId = 999;

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync((Film?)null);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.Null(result);
            _mockFilmRepository.Verify(r => r.GetFilmById(filmId), Times.Once);
        }

        [Fact]
        public async Task GetFilmById_MapsRealisateurCorrectly()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Realisateur);
            Assert.Equal(1, result.Realisateur.IdPersonne);
            Assert.Equal("Nolan", result.Realisateur.Nom);
            Assert.Equal("British-American filmmaker", result.Realisateur.Biographie);
            Assert.Equal(new DateTime(1970, 7, 30), result.Realisateur.DateNaissance);
            Assert.Equal("London", result.Realisateur.LieuNaissance);
            Assert.Equal("nolan.jpg", result.Realisateur.Photo);
        }

        [Fact]
        public async Task GetFilmById_MapsCopiesFilmCorrectly()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.CopiesFilm.Count);
            Assert.Contains(result.CopiesFilm, c => c.IdCopie == "COPY001" && c.Statut == "Disponible");
            Assert.Contains(result.CopiesFilm, c => c.IdCopie == "COPY002" && c.Statut == "Loué");
        }

        [Fact]
        public async Task GetFilmById_MapsGenresCorrectly()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.FilmsGenres.Count());
            Assert.Contains(result.FilmsGenres, g => g.IdGenre == 1 && g.NomGenre == "Action");
            Assert.Contains(result.FilmsGenres, g => g.IdGenre == 2 && g.NomGenre == "Sci-Fi");
        }

        [Fact]
        public async Task GetFilmById_MapsPaysCorrectly()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.FilmsPays.Count());
            Assert.Contains(result.FilmsPays, p => p.IdPays == 1 && p.NomPays == "United States");
            Assert.Contains(result.FilmsPays, p => p.IdPays == 2 && p.NomPays == "United Kingdom");
        }

        [Fact]
        public async Task GetFilmById_MapsActeursCorrectly()
        {
            // Arrange
            int filmId = 1;
            var film = CreateMockFilm(filmId);

            _mockFilmRepository
                .Setup(r => r.GetFilmById(filmId))
                .ReturnsAsync(film);

            // Act
            var result = await _filmService.GetFilmById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Acteurs.Count());
            Assert.Contains(result.Acteurs, a => a.IdPersonne == 2 && a.Nom == "DiCaprio" && a.NomPersonnage == "Dom Cobb");
            Assert.Contains(result.Acteurs, a => a.IdPersonne == 3 && a.Nom == "Hardy" && a.NomPersonnage == "Eames");
        }

        #endregion

        #region SearchFilmsByQuery Tests

        [Fact]
        public async Task SearchFilmsByQuery_WithValidParams_ReturnsFilmsList()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Titre = "Matrix" };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 },
                new FilmSearchDto { IdFilm = 2, Titre = "The Matrix Reloaded", Annee = 2003, Langue = "English", Duree = 138 }
            };

            _mockFilmRepository
                .Setup(r => r.SearchFilmsByQuery(searchParams))
                .ReturnsAsync(expectedFilms);

            // Act
            var result = await _filmService.SearchFilmsByQuery(searchParams);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("The Matrix", result[0].Titre);
            Assert.Equal("The Matrix Reloaded", result[1].Titre);
            _mockFilmRepository.Verify(r => r.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilmsByQuery_WithNoResults_ReturnsEmptyList()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Titre = "NonExistent" };
            var expectedFilms = new List<FilmSearchDto>();

            _mockFilmRepository
                .Setup(r => r.SearchFilmsByQuery(searchParams))
                .ReturnsAsync(expectedFilms);

            // Act
            var result = await _filmService.SearchFilmsByQuery(searchParams);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockFilmRepository.Verify(r => r.SearchFilmsByQuery(searchParams), Times.Once);
        }

        #endregion

        #region GetAllGenres Tests

        [Fact]
        public async Task GetAllGenres_ReturnsOrderedGenresList()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { IdGenre = 3, NomGenre = "Drama" },
                new Genre { IdGenre = 1, NomGenre = "Action" },
                new Genre { IdGenre = 2, NomGenre = "Sci-Fi" }
            };

            _mockFilmRepository
                .Setup(r => r.GetAllGenres())
                .ReturnsAsync(genres);

            // Act
            var result = await _filmService.GetAllGenres();

            // Assert
            Assert.NotNull(result);
            var genreList = result.ToList();
            Assert.Equal(3, genreList.Count);
            Assert.Equal(1, genreList[0].IdGenre); // Ordered by IdGenre
            Assert.Equal("Action", genreList[0].NomGenre);
            Assert.Equal(2, genreList[1].IdGenre);
            Assert.Equal("Sci-Fi", genreList[1].NomGenre);
            Assert.Equal(3, genreList[2].IdGenre);
            Assert.Equal("Drama", genreList[2].NomGenre);
            _mockFilmRepository.Verify(r => r.GetAllGenres(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenres_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var genres = new List<Genre>();

            _mockFilmRepository
                .Setup(r => r.GetAllGenres())
                .ReturnsAsync(genres);

            // Act
            var result = await _filmService.GetAllGenres();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockFilmRepository.Verify(r => r.GetAllGenres(), Times.Once);
        }

        #endregion

        #region GetAllPays Tests

        [Fact]
        public async Task GetAllPays_ReturnsOrderedPaysList()
        {
            // Arrange
            var paysList = new List<Pays>
            {
                new Pays { IdPays = 3, NomPays = "Canada" },
                new Pays { IdPays = 1, NomPays = "United States" },
                new Pays { IdPays = 2, NomPays = "France" }
            };

            _mockFilmRepository
                .Setup(r => r.GetAllPays())
                .ReturnsAsync(paysList);

            // Act
            var result = await _filmService.GetAllPays();

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            Assert.Equal(1, resultList[0].IdPays); // Ordered by IdPays
            Assert.Equal("United States", resultList[0].NomPays);
            Assert.Equal(2, resultList[1].IdPays);
            Assert.Equal("France", resultList[1].NomPays);
            Assert.Equal(3, resultList[2].IdPays);
            Assert.Equal("Canada", resultList[2].NomPays);
            _mockFilmRepository.Verify(r => r.GetAllPays(), Times.Once);
        }

        [Fact]
        public async Task GetAllPays_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var paysList = new List<Pays>();

            _mockFilmRepository
                .Setup(r => r.GetAllPays())
                .ReturnsAsync(paysList);

            // Act
            var result = await _filmService.GetAllPays();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockFilmRepository.Verify(r => r.GetAllPays(), Times.Once);
        }

        #endregion

        #region Helper Methods

        private Film CreateMockFilm(int filmId)
        {
            var realisateurPersonne = new Personne
            {
                IdPersonne = 1,
                Nom = "Nolan",
                DateNaissance = new DateTime(1970, 7, 30),
                LieuNaissance = "London",
                Photo = "nolan.jpg",
                Biographie = "British-American filmmaker"
            };

            var realisateur = new Realisateur
            {
                IdPersonne = 1,
                Personne = realisateurPersonne
            };

            var acteur1Personne = new Personne
            {
                IdPersonne = 2,
                Nom = "DiCaprio",
                Photo = "dicaprio.jpg"
            };

            var acteur1 = new Acteur
            {
                IdPersonne = 2,
                Personne = acteur1Personne
            };

            var acteur2Personne = new Personne
            {
                IdPersonne = 3,
                Nom = "Hardy",
                Photo = "hardy.jpg"
            };

            var acteur2 = new Acteur
            {
                IdPersonne = 3,
                Personne = acteur2Personne
            };

            var film = new Film
            {
                IdFilm = filmId,
                Titre = "Inception",
                Annee = 2010,
                Langue = "English",
                Duree = 148,
                Resume = "A thief who steals corporate secrets through dream-sharing technology.",
                Affiche = "inception.jpg",
                BandeAnnonce = "inception_trailer.mp4",
                IdRealisateur = 1,
                Realisateur = realisateur,
                CopiesFilm = new List<CopieFilm>
                {
                    new CopieFilm { IdCopie = "COPY001", IdFilm = filmId, Statut = "Disponible" },
                    new CopieFilm { IdCopie = "COPY002", IdFilm = filmId, Statut = "Loué" }
                },
                FilmsGenres = new List<FilmGenre>
                {
                    new FilmGenre { IdFilm = filmId, IdGenre = 1, Genre = new Genre { IdGenre = 1, NomGenre = "Action" } },
                    new FilmGenre { IdFilm = filmId, IdGenre = 2, Genre = new Genre { IdGenre = 2, NomGenre = "Sci-Fi" } }
                },
                FilmsPays = new List<FilmPays>
                {
                    new FilmPays { IdFilm = filmId, IdPays = 1, Pays = new Pays { IdPays = 1, NomPays = "United States" } },
                    new FilmPays { IdFilm = filmId, IdPays = 2, Pays = new Pays { IdPays = 2, NomPays = "United Kingdom" } }
                },
                Roles = new List<Role>
                {
                    new Role { IdFilm = filmId, IdActeur = 2, NomPersonnage = "Dom Cobb", Acteur = acteur1 },
                    new Role { IdFilm = filmId, IdActeur = 3, NomPersonnage = "Eames", Acteur = acteur2 }
                }
            };

            return film;
        }

        #endregion
    }
}