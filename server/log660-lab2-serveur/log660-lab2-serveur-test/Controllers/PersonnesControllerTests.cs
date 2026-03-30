using log660_lab2_serveur.Controllers;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace log660_lab2_serveur_test.Controllers
{
    public class PersonnesControllerTests
    {
        private readonly Mock<IPersonneService> _mockPersonneService;
        private readonly PersonnesController _controller;

        public PersonnesControllerTests()
        {
            _mockPersonneService = new Mock<IPersonneService>();
            _controller = new PersonnesController(_mockPersonneService.Object);
        }

        #region GetById Tests

        [Fact]
        public async Task GetById_WithValidId_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 1;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Nolan",
                DateNaissance = new DateTime(1970, 7, 30),
                LieuNaissance = "London, England",
                Photo = "nolan.jpg",
                Biographie = "Christopher Nolan is a British-American film director.",
                EstActeur = false,
                EstRealisateur = true
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(expectedPersonne.IdPersonne, returnedPersonne.IdPersonne);
            Assert.Equal(expectedPersonne.Nom, returnedPersonne.Nom);
            Assert.Equal(expectedPersonne.DateNaissance, returnedPersonne.DateNaissance);
            Assert.Equal(expectedPersonne.LieuNaissance, returnedPersonne.LieuNaissance);
            Assert.Equal(expectedPersonne.Photo, returnedPersonne.Photo);
            Assert.Equal(expectedPersonne.Biographie, returnedPersonne.Biographie);
            Assert.Equal(expectedPersonne.EstActeur, returnedPersonne.EstActeur);
            Assert.Equal(expectedPersonne.EstRealisateur, returnedPersonne.EstRealisateur);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithValidActeur_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 2;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "DiCaprio",
                DateNaissance = new DateTime(1974, 11, 11),
                LieuNaissance = "Los Angeles, California",
                Photo = "dicaprio.jpg",
                Biographie = "Leonardo DiCaprio is an American actor and film producer.",
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(expectedPersonne.IdPersonne, returnedPersonne.IdPersonne);
            Assert.True(returnedPersonne.EstActeur);
            Assert.False(returnedPersonne.EstRealisateur);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithPersonneWhoIsBothActeurAndRealisateur_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 3;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Eastwood",
                DateNaissance = new DateTime(1930, 5, 31),
                LieuNaissance = "San Francisco, California",
                Photo = "eastwood.jpg",
                Biographie = "Clint Eastwood is an American actor and film director.",
                EstActeur = true,
                EstRealisateur = true
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.True(returnedPersonne.EstActeur);
            Assert.True(returnedPersonne.EstRealisateur);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNullOptionalFields_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 4;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Unknown Actor",
                DateNaissance = null,
                LieuNaissance = null,
                Photo = null,
                Biographie = null,
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(expectedPersonne.IdPersonne, returnedPersonne.IdPersonne);
            Assert.Equal(expectedPersonne.Nom, returnedPersonne.Nom);
            Assert.Null(returnedPersonne.DateNaissance);
            Assert.Null(returnedPersonne.LieuNaissance);
            Assert.Null(returnedPersonne.Photo);
            Assert.Null(returnedPersonne.Biographie);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            int idPersonne = 999;

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync((PersonneDto?)null);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Personne introuvable.", notFoundResult.Value);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithZeroId_CallsServiceAndReturnsNotFound()
        {
            // Arrange
            int idPersonne = 0;

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync((PersonneDto?)null);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Personne introuvable.", notFoundResult.Value);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNegativeId_CallsServiceAndReturnsNotFound()
        {
            // Arrange
            int idPersonne = -1;

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync((PersonneDto?)null);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Personne introuvable.", notFoundResult.Value);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithMaxIntId_CallsService()
        {
            // Arrange
            int idPersonne = int.MaxValue;

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync((PersonneDto?)null);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_CallsServiceExactlyOnce()
        {
            // Arrange
            int idPersonne = 1;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Test Person",
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            await _controller.GetById(idPersonne);

            // Assert
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
            _mockPersonneService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetById_WithMultipleSuccessiveRequests_HandlesEachIndependently()
        {
            // Arrange
            int idPersonne1 = 1;
            int idPersonne2 = 2;

            var personne1 = new PersonneDto
            {
                IdPersonne = idPersonne1,
                Nom = "Person 1",
                EstActeur = true,
                EstRealisateur = false
            };

            var personne2 = new PersonneDto
            {
                IdPersonne = idPersonne2,
                Nom = "Person 2",
                EstActeur = false,
                EstRealisateur = true
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne1))
                .ReturnsAsync(personne1);

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne2))
                .ReturnsAsync(personne2);

            // Act
            var result1 = await _controller.GetById(idPersonne1);
            var result2 = await _controller.GetById(idPersonne2);

            // Assert
            var okResult1 = Assert.IsType<OkObjectResult>(result1);
            var okResult2 = Assert.IsType<OkObjectResult>(result2);

            var returnedPersonne1 = Assert.IsType<PersonneDto>(okResult1.Value);
            var returnedPersonne2 = Assert.IsType<PersonneDto>(okResult2.Value);

            Assert.Equal(personne1.Nom, returnedPersonne1.Nom);
            Assert.Equal(personne2.Nom, returnedPersonne2.Nom);

            _mockPersonneService.Verify(s => s.GetById(idPersonne1), Times.Once);
            _mockPersonneService.Verify(s => s.GetById(idPersonne2), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsIActionResult()
        {
            // Arrange
            int idPersonne = 1;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Test Person",
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public async Task GetById_WithPersonneHavingVeryOldBirthDate_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 5;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Classic Actor",
                DateNaissance = new DateTime(1900, 1, 1),
                LieuNaissance = "Old Town",
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(new DateTime(1900, 1, 1), returnedPersonne.DateNaissance);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithPersonneHavingFutureBirthDate_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 6;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Future Person",
                DateNaissance = new DateTime(2050, 12, 31),
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(new DateTime(2050, 12, 31), returnedPersonne.DateNaissance);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithEmptyNom_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 7;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = string.Empty,
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(string.Empty, returnedPersonne.Nom);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithPersonneWhoIsNeitherActeurNorRealisateur_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 8;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Other Role Person",
                DateNaissance = new DateTime(1985, 5, 15),
                EstActeur = false,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.False(returnedPersonne.EstActeur);
            Assert.False(returnedPersonne.EstRealisateur);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithLongBiographie_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 9;
            var longBiographie = new string('A', 5000);
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "Person with Long Bio",
                Biographie = longBiographie,
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal(longBiographie, returnedPersonne.Biographie);
            Assert.Equal(5000, returnedPersonne.Biographie?.Length);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        [Fact]
        public async Task GetById_WithSpecialCharactersInFields_ReturnsOkWithPersonne()
        {
            // Arrange
            int idPersonne = 10;
            var expectedPersonne = new PersonneDto
            {
                IdPersonne = idPersonne,
                Nom = "O'Brien-Müller",
                LieuNaissance = "São Paulo, Brazil",
                Biographie = "Famous for roles in \"Action & Drama\" films!",
                EstActeur = true,
                EstRealisateur = false
            };

            _mockPersonneService
                .Setup(s => s.GetById(idPersonne))
                .ReturnsAsync(expectedPersonne);

            // Act
            var result = await _controller.GetById(idPersonne);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersonne = Assert.IsType<PersonneDto>(okResult.Value);
            Assert.Equal("O'Brien-Müller", returnedPersonne.Nom);
            Assert.Equal("São Paulo, Brazil", returnedPersonne.LieuNaissance);
            Assert.Contains("&", returnedPersonne.Biographie);
            _mockPersonneService.Verify(s => s.GetById(idPersonne), Times.Once);
        }

        #endregion
    }
}
