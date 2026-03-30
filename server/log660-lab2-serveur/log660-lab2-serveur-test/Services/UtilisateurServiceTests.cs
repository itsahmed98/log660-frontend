using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;
using log660_lab2_serveur.Services;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Services
{
    public class UtilisateurServiceTests
    {
        private readonly Mock<IUtilisateurRepository> _mockUtilisateurRepository;
        private readonly UtilisateurService _utilisateurService;

        public UtilisateurServiceTests()
        {
            _mockUtilisateurRepository = new Mock<IUtilisateurRepository>();
            _utilisateurService = new UtilisateurService(_mockUtilisateurRepository.Object);
        }

        #region LoginAsync Tests

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsUtilisateurDto()
        {
            // Arrange
            string courriel = "john.doe@example.com";
            string motDePasse = "password123";
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 1,
                Nom = "Doe",
                Prenom = "John",
                Courriel = courriel,
                Telephone = "5141234567",
                DateNaissance = new DateTime(1990, 5, 15),
                MotDePasse = motDePasse
            };

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync(utilisateur);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(utilisateur.IdUtilisateur, result.IdUtilisateur);
            Assert.Equal(utilisateur.Nom, result.Nom);
            Assert.Equal(utilisateur.Prenom, result.Prenom);
            Assert.Equal(utilisateur.Courriel, result.Courriel);
            Assert.Equal(utilisateur.Telephone, result.Telephone);
            Assert.Equal(utilisateur.DateNaissance, result.DateNaissance);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            string courriel = "invalid@example.com";
            string motDePasse = "wrongpassword";

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync((Utilisateur?)null);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.Null(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentEmail_ReturnsNull()
        {
            // Arrange
            string courriel = "nonexistent@example.com";
            string motDePasse = "password123";

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync((Utilisateur?)null);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.Null(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithEmptyEmail_CallsRepository()
        {
            // Arrange
            string courriel = "";
            string motDePasse = "password123";

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync((Utilisateur?)null);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.Null(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithEmptyPassword_CallsRepository()
        {
            // Arrange
            string courriel = "user@example.com";
            string motDePasse = "";

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync((Utilisateur?)null);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.Null(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_MapsAllFieldsCorrectly()
        {
            // Arrange
            string courriel = "jane.smith@example.com";
            string motDePasse = "securePass";
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 42,
                Nom = "Smith",
                Prenom = "Jane",
                Courriel = courriel,
                Telephone = "4381234567",
                DateNaissance = new DateTime(1985, 12, 25),
                MotDePasse = motDePasse
            };

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync(utilisateur);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UtilisateurDto>(result);
            Assert.Equal(42, result.IdUtilisateur);
            Assert.Equal("Smith", result.Nom);
            Assert.Equal("Jane", result.Prenom);
            Assert.Equal(courriel, result.Courriel);
            Assert.Equal("4381234567", result.Telephone);
            Assert.Equal(new DateTime(1985, 12, 25), result.DateNaissance);
        }

        [Fact]
        public async Task LoginAsync_WithSpecialCharactersInEmail_CallsRepository()
        {
            // Arrange
            string courriel = "user+test@example.co.uk";
            string motDePasse = "password";
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 1,
                Nom = "Test",
                Prenom = "User",
                Courriel = courriel,
                Telephone = "1234567890",
                DateNaissance = new DateTime(1995, 1, 1),
                MotDePasse = motDePasse
            };

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync(utilisateur);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courriel, result.Courriel);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithCaseSensitiveEmail_CallsRepository()
        {
            // Arrange
            string courriel = "User@Example.COM";
            string motDePasse = "password";

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync((Utilisateur?)null);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.Null(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithLongPassword_CallsRepository()
        {
            // Arrange
            string courriel = "user@example.com";
            string motDePasse = new string('a', 30);
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 1,
                Nom = "User",
                Prenom = "Test",
                Courriel = courriel,
                Telephone = "1234567890",
                DateNaissance = new DateTime(1990, 1, 1),
                MotDePasse = motDePasse
            };

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync(utilisateur);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.NotNull(result);
            _mockUtilisateurRepository.Verify(r => r.GetUtilisateurByEmail(courriel, motDePasse), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_DoesNotReturnPasswordInDto()
        {
            // Arrange
            string courriel = "secure@example.com";
            string motDePasse = "secretPassword123";
            var utilisateur = new Utilisateur
            {
                IdUtilisateur = 1,
                Nom = "Secure",
                Prenom = "User",
                Courriel = courriel,
                Telephone = "1234567890",
                DateNaissance = new DateTime(1990, 1, 1),
                MotDePasse = motDePasse
            };

            _mockUtilisateurRepository
                .Setup(r => r.GetUtilisateurByEmail(courriel, motDePasse))
                .ReturnsAsync(utilisateur);

            // Act
            var result = await _utilisateurService.LoginAsync(courriel, motDePasse);

            // Assert
            Assert.NotNull(result);
            var dtoType = result.GetType();
            var passwordProperty = dtoType.GetProperty("MotDePasse");
            Assert.Null(passwordProperty);
        }

        #endregion
    }
}