using log660_lab2_serveur.Controllers;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace log660_lab2_serveur_test.Controllers
{
    public class LocationsControllerTests
    {
        private readonly Mock<ILocationService> _mockLocationService;
        private readonly LocationsController _controller;

        public LocationsControllerTests()
        {
            _mockLocationService = new Mock<ILocationService>();
            _controller = new LocationsController(_mockLocationService.Object);
        }

        #region Louer Tests

        [Fact]
        public async Task Louer_WithValidParameters_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 200,
                Message = "Location effectuée avec succès."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(expectedResponse.ResponseCode, returnedResponse.ResponseCode);
            Assert.Equal(expectedResponse.Message, returnedResponse.Message);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithNonExistentUser_ReturnsOkWithErrorResponse()
        {
            // Arrange
            int idUtilisateur = 999;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 404,
                Message = "Utilisateur introuvable."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(404, returnedResponse.ResponseCode);
            Assert.Equal(expectedResponse.Message, returnedResponse.Message);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithNonExistentCopy_ReturnsOkWithErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "NONEXISTENT";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 404,
                Message = "Copie introuvable."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(404, returnedResponse.ResponseCode);
            Assert.Equal(expectedResponse.Message, returnedResponse.Message);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithUnavailableCopy_ReturnsOkWithErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 400,
                Message = "La copie n'est pas disponible pour la location."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(400, returnedResponse.ResponseCode);
            Assert.Contains("disponible", returnedResponse.Message.ToLower());
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithZeroUserId_CallsService()
        {
            // Arrange
            int idUtilisateur = 0;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 400,
                Message = "Identifiant utilisateur invalide."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(400, returnedResponse.ResponseCode);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithNegativeUserId_CallsService()
        {
            // Arrange
            int idUtilisateur = -1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 400,
                Message = "Identifiant utilisateur invalide."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithEmptyIdCopie_CallsService()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 400,
                Message = "Identifiant de copie invalide."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithWhitespaceIdCopie_CallsService()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "   ";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 400,
                Message = "Identifiant de copie invalide."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithSpecialCharactersInIdCopie_CallsService()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY@#$%";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 200,
                Message = "Location effectuée avec succès."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(expectedResponse.ResponseCode, returnedResponse.ResponseCode);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithLongIdCopie_CallsService()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = new string('A', 100);
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 200,
                Message = "Location effectuée avec succès."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithMaxIntUserId_CallsService()
        {
            // Arrange
            int idUtilisateur = int.MaxValue;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 404,
                Message = "Utilisateur introuvable."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_WithInternalServerError_ReturnsOkWithErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 500,
                Message = "Une erreur interne est survenue."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.Equal(500, returnedResponse.ResponseCode);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
        }

        [Fact]
        public async Task Louer_CallsServiceExactlyOnce_ForEachRequest()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 200,
                Message = "Location effectuée avec succès."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur, idCopie), Times.Once);
            _mockLocationService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Louer_ReturnsActionResultOfResponseDto()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var expectedResponse = new ResponseDto
            {
                ResponseCode = 200,
                Message = "Location effectuée avec succès."
            };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur, idCopie))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Louer(idUtilisateur, idCopie);

            // Assert
            Assert.IsType<ActionResult<ResponseDto>>(result);
        }

        [Fact]
        public async Task Louer_WithMultipleSuccessiveRequests_HandlesEachIndependently()
        {
            // Arrange
            int idUtilisateur1 = 1;
            string idCopie1 = "COPY001";
            int idUtilisateur2 = 2;
            string idCopie2 = "COPY002";

            var response1 = new ResponseDto { ResponseCode = 200, Message = "Location 1 réussie." };
            var response2 = new ResponseDto { ResponseCode = 200, Message = "Location 2 réussie." };

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur1, idCopie1))
                .ReturnsAsync(response1);

            _mockLocationService
                .Setup(s => s.LouerAsync(idUtilisateur2, idCopie2))
                .ReturnsAsync(response2);

            // Act
            var result1 = await _controller.Louer(idUtilisateur1, idCopie1);
            var result2 = await _controller.Louer(idUtilisateur2, idCopie2);

            // Assert
            var okResult1 = Assert.IsType<OkObjectResult>(result1.Result);
            var okResult2 = Assert.IsType<OkObjectResult>(result2.Result);

            var returnedResponse1 = Assert.IsType<ResponseDto>(okResult1.Value);
            var returnedResponse2 = Assert.IsType<ResponseDto>(okResult2.Value);

            Assert.Equal(response1.Message, returnedResponse1.Message);
            Assert.Equal(response2.Message, returnedResponse2.Message);

            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur1, idCopie1), Times.Once);
            _mockLocationService.Verify(s => s.LouerAsync(idUtilisateur2, idCopie2), Times.Once);
        }

        #endregion
    }
}
