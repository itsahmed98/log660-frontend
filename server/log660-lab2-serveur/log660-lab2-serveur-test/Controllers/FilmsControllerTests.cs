using log660_lab2_serveur.Controllers;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Controllers
{
    public class FilmsControllerTests
    {
        private readonly Mock<IFilmService> _mockFilmService;
        private readonly FilmsController _controller;

        public FilmsControllerTests()
        {
            _mockFilmService = new Mock<IFilmService>();
            _controller = new FilmsController(_mockFilmService.Object);
        }

        #region GetFilmById Tests

        [Fact]
        public async Task GetFilmById_WithValidId_ReturnsOkWithFilm()
        {
            // Arrange
            int filmId = 1;
            var expectedFilm = new FilmDto
            {
                IdFilm = filmId,
                Titre = "The Matrix",
                Annee = 1999,
                Langue = "English",
                Duree = 136,
                Resume = "A computer hacker learns about the true nature of reality.",
                Affiche = "matrix.jpg",
                BandeAnnonce = "matrix_trailer.mp4"
            };

            _mockFilmService
                .Setup(s => s.GetFilmById(filmId))
                .Returns(Task.FromResult<FilmDto?>(expectedFilm));

            // Act
            var result = await _controller.GetFilmById(filmId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFilm = Assert.IsType<FilmDto>(okResult.Value);
            Assert.Equal(expectedFilm.IdFilm, returnedFilm.IdFilm);
            Assert.Equal(expectedFilm.Titre, returnedFilm.Titre);
            Assert.Equal(expectedFilm.Annee, returnedFilm.Annee);
            _mockFilmService.Verify(s => s.GetFilmById(filmId), Times.Once);
        }

        [Fact]
        public async Task GetFilmById_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            int filmId = 999;

            _mockFilmService
                .Setup(s => s.GetFilmById(filmId))
                .Returns(Task.FromResult<FilmDto?>(null));

            // Act
            var result = await _controller.GetFilmById(filmId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Film introuvable dans le system.", notFoundResult.Value);
            _mockFilmService.Verify(s => s.GetFilmById(filmId), Times.Once);
        }

        [Fact]
        public async Task GetFilmById_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            int filmId = 1;
            var exceptionMessage = "Database connection failed";

            _mockFilmService
                .Setup(s => s.GetFilmById(filmId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetFilmById(filmId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains(exceptionMessage, statusCodeResult.Value?.ToString());
            _mockFilmService.Verify(s => s.GetFilmById(filmId), Times.Once);
        }

        #endregion

        #region SearchFilms Tests

        [Fact]
        public async Task SearchFilms_WithValidTitre_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Titre = "Matrix" };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 },
                new FilmSearchDto { IdFilm = 2, Titre = "The Matrix Reloaded", Annee = 2003, Langue = "English", Duree = 138 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFilms = Assert.IsType<List<FilmSearchDto>>(okResult.Value);
            Assert.Equal(2, returnedFilms.Count);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidActeur_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Acteur = "Keanu Reeves" };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFilms = Assert.IsType<List<FilmSearchDto>>(okResult.Value);
            Assert.Single(returnedFilms);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidRealisateur_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Realisateur = "Wachowski" };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidIdGenre_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { IdGenre = 1 };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidIdPays_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { IdPays = 1 };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidLangue_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Langue = "English" };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithValidYearRange_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams { MinAnnee = 1990, MaxAnnee = 2000 };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithMultipleCriteria_ReturnsOkWithResults()
        {
            // Arrange
            var searchParams = new FilmSearchParams
            {
                Titre = "Matrix",
                Acteur = "Keanu Reeves",
                MinAnnee = 1990,
                MaxAnnee = 2000
            };
            var expectedFilms = new List<FilmSearchDto>
            {
                new FilmSearchDto { IdFilm = 1, Titre = "The Matrix", Annee = 1999, Langue = "English", Duree = 136 }
            };

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        [Fact]
        public async Task SearchFilms_WithNoCriteria_ReturnsBadRequest()
        {
            // Arrange
            var searchParams = new FilmSearchParams();

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Au moins un critère de recherche est requis.", badRequestResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(It.IsAny<FilmSearchParams>()), Times.Never);
        }

        [Fact]
        public async Task SearchFilms_WithMaxAnneeBeforeMinAnnee_ReturnsBadRequest()
        {
            // Arrange
            var searchParams = new FilmSearchParams { MinAnnee = 2000, MaxAnnee = 1990 };

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("La date de début ne peux pas être apres la date de fin.", badRequestResult.Value);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(It.IsAny<FilmSearchParams>()), Times.Never);
        }

        [Fact]
        public async Task SearchFilms_ReturnsEmptyList_WhenNoResultsFound()
        {
            // Arrange
            var searchParams = new FilmSearchParams { Titre = "NonExistentFilm" };
            var expectedFilms = new List<FilmSearchDto>();

            _mockFilmService
                .Setup(s => s.SearchFilmsByQuery(searchParams))
                .Returns(Task.FromResult(expectedFilms));

            // Act
            var result = await _controller.SearchFilms(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFilms = Assert.IsType<List<FilmSearchDto>>(okResult.Value);
            Assert.Empty(returnedFilms);
            _mockFilmService.Verify(s => s.SearchFilmsByQuery(searchParams), Times.Once);
        }

        #endregion

        #region GetLangues Tests

        [Fact]
        public void GetLangues_ReturnsListOfLanguages()
        {
            // Act
            var result = _controller.GetLangues();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<string>>>(result);
            var languages = Assert.IsType<List<string>>(actionResult.Value);
            Assert.Equal(12, languages.Count);
            Assert.Contains("English", languages);
            Assert.Contains("Frensh", languages);
            Assert.Contains("Portuguese", languages);
            Assert.Contains("Cantonese", languages);
            Assert.Contains("Japanese", languages);
            Assert.Contains("German", languages);
            Assert.Contains("Italian", languages);
            Assert.Contains("Swedish", languages);
            Assert.Contains("Mandarin", languages);
            Assert.Contains("Spanish", languages);
            Assert.Contains("Korean", languages);
            Assert.Contains("Aramaic", languages);
        }

        [Fact]
        public void GetLangues_ReturnsNonEmptyList()
        {
            // Act
            var result = _controller.GetLangues();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<string>>>(result);
            var languages = Assert.IsType<List<string>>(actionResult.Value);
            Assert.NotEmpty(languages);
        }

        #endregion

        #region GetAllGenres Tests

        [Fact]
        public async Task GetAllGenres_ReturnsListOfGenres()
        {
            // Arrange
            var expectedGenres = new List<GenreDto>
            {
                new GenreDto { IdGenre = 1, NomGenre = "Action" },
                new GenreDto { IdGenre = 2, NomGenre = "Sci-Fi" },
                new GenreDto { IdGenre = 3, NomGenre = "Drama" }
            };

            _mockFilmService
                .Setup(s => s.GetAllGenres())
                .Returns(Task.FromResult<IEnumerable<GenreDto>>(expectedGenres));

            // Act
            var result = await _controller.GetAllGenres();

            // Assert
            var genres = Assert.IsAssignableFrom<IEnumerable<GenreDto>>(result);
            var genreList = genres.ToList();
            Assert.Equal(3, genreList.Count);
            Assert.Equal("Action", genreList[0].NomGenre);
            Assert.Equal("Sci-Fi", genreList[1].NomGenre);
            Assert.Equal("Drama", genreList[2].NomGenre);
            _mockFilmService.Verify(s => s.GetAllGenres(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenres_ReturnsEmptyList_WhenNoGenresExist()
        {
            // Arrange
            var expectedGenres = new List<GenreDto>();

            _mockFilmService
                .Setup(s => s.GetAllGenres())
                .Returns(Task.FromResult<IEnumerable<GenreDto>>(expectedGenres));

            // Act
            var result = await _controller.GetAllGenres();

            // Assert
            var genres = Assert.IsAssignableFrom<IEnumerable<GenreDto>>(result);
            Assert.Empty(genres);
            _mockFilmService.Verify(s => s.GetAllGenres(), Times.Once);
        }

        #endregion

        #region GetAllPays Tests

        [Fact]
        public async Task GetAllPays_ReturnsListOfCountries()
        {
            // Arrange
            var expectedPays = new List<PaysDto>
            {
                new PaysDto { IdPays = 1, NomPays = "United States" },
                new PaysDto { IdPays = 2, NomPays = "France" },
                new PaysDto { IdPays = 3, NomPays = "Canada" }
            };

            _mockFilmService
                .Setup(s => s.GetAllPays())
                .Returns(Task.FromResult<IEnumerable<PaysDto>>(expectedPays));

            // Act
            var result = await _controller.GetAllPays();

            // Assert
            var pays = Assert.IsAssignableFrom<IEnumerable<PaysDto>>(result);
            var paysList = pays.ToList();
            Assert.Equal(3, paysList.Count);
            Assert.Equal("United States", paysList[0].NomPays);
            Assert.Equal("France", paysList[1].NomPays);
            Assert.Equal("Canada", paysList[2].NomPays);
            _mockFilmService.Verify(s => s.GetAllPays(), Times.Once);
        }

        [Fact]
        public async Task GetAllPays_ReturnsEmptyList_WhenNoCountriesExist()
        {
            // Arrange
            var expectedPays = new List<PaysDto>();

            _mockFilmService
                .Setup(s => s.GetAllPays())
                .Returns(Task.FromResult<IEnumerable<PaysDto>>(expectedPays));

            // Act
            var result = await _controller.GetAllPays();

            // Assert
            var pays = Assert.IsAssignableFrom<IEnumerable<PaysDto>>(result);
            Assert.Empty(pays);
            _mockFilmService.Verify(s => s.GetAllPays(), Times.Once);
        }

        #endregion
    }
}