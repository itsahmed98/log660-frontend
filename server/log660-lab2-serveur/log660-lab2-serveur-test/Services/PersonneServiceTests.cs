using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Models;
using log660_lab2_serveur.Services;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Services
{
    public class PersonneServiceTests
    {
        private readonly Mock<IPersonneRepository> _mockPersonneRepository;
        private readonly PersonneService _personneService;

        public PersonneServiceTests()
        {
            _mockPersonneRepository = new Mock<IPersonneRepository>();
            _personneService = new PersonneService(_mockPersonneRepository.Object);
        }

        #region GetById Tests

        [Fact]
        public async Task GetById_WithValidActeur_ReturnsPersonneDtoWithEstActeurTrue()
        {
            // Arrange
            int idPersonne = 1;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "DiCaprio",
                DateNaissance = new DateTime(1974, 11, 11),
                LieuNaissance = "Los Angeles",
                Photo = "dicaprio.jpg",
                Biographie = "American actor",
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idPersonne, result.IdPersonne);
            Assert.Equal("DiCaprio", result.Nom);
            Assert.Equal(new DateTime(1974, 11, 11), result.DateNaissance);
            Assert.Equal("Los Angeles", result.LieuNaissance);
            Assert.Equal("dicaprio.jpg", result.Photo);
            Assert.Equal("American actor", result.Biographie);
            Assert.True(result.EstActeur);
            Assert.False(result.EstRealisateur);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithValidRealisateur_ReturnsPersonneDtoWithEstRealisateurTrue()
        {
            // Arrange
            int idPersonne = 2;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Nolan",
                DateNaissance = new DateTime(1970, 7, 30),
                LieuNaissance = "London",
                Photo = "nolan.jpg",
                Biographie = "British-American filmmaker",
                Acteur = null,
                Realisateur = new Realisateur { IdPersonne = idPersonne }
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idPersonne, result.IdPersonne);
            Assert.Equal("Nolan", result.Nom);
            Assert.False(result.EstActeur);
            Assert.True(result.EstRealisateur);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithBothActeurAndRealisateur_ReturnsBothTrue()
        {
            // Arrange
            int idPersonne = 3;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Eastwood",
                DateNaissance = new DateTime(1930, 5, 31),
                LieuNaissance = "San Francisco",
                Photo = "eastwood.jpg",
                Biographie = "American actor and director",
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = new Realisateur { IdPersonne = idPersonne }
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.EstActeur);
            Assert.True(result.EstRealisateur);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNeitherActeurNorRealisateur_ReturnsBothFalse()
        {
            // Arrange
            int idPersonne = 4;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Writer",
                DateNaissance = new DateTime(1980, 1, 1),
                LieuNaissance = "New York",
                Photo = "writer.jpg",
                Biographie = "Screenwriter",
                Acteur = null,
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.EstActeur);
            Assert.False(result.EstRealisateur);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            int idPersonne = 999;

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync((Personne?)null);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.Null(result);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNullOptionalFields_MapsCorrectly()
        {
            // Arrange
            int idPersonne = 5;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "MinimalData",
                DateNaissance = null,
                LieuNaissance = null,
                Photo = null,
                Biographie = null,
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idPersonne, result.IdPersonne);
            Assert.Equal("MinimalData", result.Nom);
            Assert.Null(result.DateNaissance);
            Assert.Null(result.LieuNaissance);
            Assert.Null(result.Photo);
            Assert.Null(result.Biographie);
            Assert.True(result.EstActeur);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_MapsAllFieldsCorrectly()
        {
            // Arrange
            int idPersonne = 6;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Test Person",
                DateNaissance = new DateTime(1985, 3, 15),
                LieuNaissance = "Paris, France",
                Photo = "test.jpg",
                Biographie = "A test biography with lots of details",
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.IdPersonne);
            Assert.Equal("Test Person", result.Nom);
            Assert.Equal(new DateTime(1985, 3, 15), result.DateNaissance);
            Assert.Equal("Paris, France", result.LieuNaissance);
            Assert.Equal("test.jpg", result.Photo);
            Assert.Equal("A test biography with lots of details", result.Biographie);
            Assert.True(result.EstActeur);
            Assert.False(result.EstRealisateur);
        }

        [Fact]
        public async Task GetById_WithZeroId_CallsRepository()
        {
            // Arrange
            int idPersonne = 0;

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync((Personne?)null);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.Null(result);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNegativeId_CallsRepository()
        {
            // Arrange
            int idPersonne = -1;

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync((Personne?)null);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.Null(result);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithSpecialCharactersInName_MapsCorrectly()
        {
            // Arrange
            int idPersonne = 7;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "O'Brien-Müller",
                DateNaissance = new DateTime(1990, 1, 1),
                LieuNaissance = "São Paulo",
                Photo = "obrien.jpg",
                Biographie = "Actor with special characters in name",
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("O'Brien-Müller", result.Nom);
            Assert.Equal("São Paulo", result.LieuNaissance);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithVeryOldBirthdate_MapsCorrectly()
        {
            // Arrange
            int idPersonne = 8;
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Classic Actor",
                DateNaissance = new DateTime(1900, 1, 1),
                LieuNaissance = "Old City",
                Photo = "classic.jpg",
                Biographie = "Very old actor",
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new DateTime(1900, 1, 1), result.DateNaissance);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithLongBiography_MapsCorrectly()
        {
            // Arrange
            int idPersonne = 9;
            string longBio = new string('A', 5000);
            var personne = new Personne
            {
                IdPersonne = idPersonne,
                Nom = "Verbose Actor",
                DateNaissance = new DateTime(1980, 1, 1),
                LieuNaissance = "City",
                Photo = "actor.jpg",
                Biographie = longBio,
                Acteur = new Acteur { IdPersonne = idPersonne },
                Realisateur = null
            };

            _mockPersonneRepository
                .Setup(r => r.GetById(idPersonne))
                .ReturnsAsync(personne);

            // Act
            var result = await _personneService.GetById(idPersonne);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(longBio, result.Biographie);
            Assert.Equal(5000, result.Biographie?.Length);
            _mockPersonneRepository.Verify(r => r.GetById(idPersonne), Times.Once);
        }

        #endregion
    }
}