using log660_lab2_serveur.Controllers;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUtilisateurService> _mockUtilisateurService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUtilisateurService = new Mock<IUtilisateurService>();
            _controller = new AuthController(_mockUtilisateurService.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithUser()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "test@example.com",
                MotDePasse = "password123"
            };

            var expectedUser = new UtilisateurDto
            {
                IdUtilisateur = 1,
                Nom = "Doe",
                Prenom = "John",
                Courriel = "test@example.com",
                Telephone = "1234567890",
                DateNaissance = new DateTime(1990, 1, 1)
            };

            _mockUtilisateurService
                .Setup(s => s.LoginAsync(request.Courriel, request.MotDePasse))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UtilisateurDto>(okResult.Value);
            Assert.Equal(expectedUser.IdUtilisateur, returnedUser.IdUtilisateur);
            Assert.Equal(expectedUser.Courriel, returnedUser.Courriel);
            _mockUtilisateurService.Verify(s => s.LoginAsync(request.Courriel, request.MotDePasse), Times.Once);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "test@example.com",
                MotDePasse = "wrongpassword"
            };

            _mockUtilisateurService
                .Setup(s => s.LoginAsync(request.Courriel, request.MotDePasse))
                .ReturnsAsync((UtilisateurDto?)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Utilisateur introuvable ou mot de passe incorrect.", unauthorizedResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(request.Courriel, request.MotDePasse), Times.Once);
        }

        [Fact]
        public async Task Login_WithEmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "",
                MotDePasse = "password123"
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithNullEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = null!,
                MotDePasse = "password123"
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithWhitespaceEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "   ",
                MotDePasse = "password123"
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "test@example.com",
                MotDePasse = ""
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithNullPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "test@example.com",
                MotDePasse = null!
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithWhitespacePassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "test@example.com",
                MotDePasse = "   "
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithBothFieldsEmpty_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Courriel = "",
                MotDePasse = ""
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Courriel et mot de passe requis.", badRequestResult.Value);
            _mockUtilisateurService.Verify(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}