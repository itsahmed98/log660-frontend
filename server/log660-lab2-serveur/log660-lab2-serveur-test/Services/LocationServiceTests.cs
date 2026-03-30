using System;
using System.Collections.Generic;
using System.Text;
using log660_lab2_serveur.Data;
using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Models;
using log660_lab2_serveur.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace log660_lab2_serveur_test.Services
{
    public class LocationServiceTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<IDbContextTransaction> _mockTransaction;
        private readonly Mock<DatabaseFacade> _mockDatabase;
        private readonly LocationService _locationService;

        public LocationServiceTests()
        {
            // Create options for in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create the mock without passing options
            _mockDbContext = new Mock<AppDbContext>(options) { CallBase = true };
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockTransaction = new Mock<IDbContextTransaction>();
            _mockDatabase = new Mock<DatabaseFacade>(_mockDbContext.Object);

            // Setup Database property and transaction
            _mockDbContext.Setup(db => db.Database).Returns(_mockDatabase.Object);
            _mockDatabase.Setup(db => db.BeginTransactionAsync(default))
                .ReturnsAsync(_mockTransaction.Object);

            _locationService = new LocationService(_mockDbContext.Object, _mockLocationRepository.Object);
        }

        #region LouerAsync Tests

        [Fact]
        public async Task LouerAsync_WithValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>())).Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ResponseCode);
            Assert.Equal("Film loué avec succès.", result.Message);
            _mockLocationRepository.Verify(r => r.AddLocation(It.IsAny<Location>()), Times.Once);
            _mockLocationRepository.Verify(r => r.SaveChanges(), Times.Once);
            _mockTransaction.Verify(t => t.CommitAsync(default), Times.Once);
        }

        [Fact]
        public async Task LouerAsync_WithCopieAlreadyLoued_ReturnsErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Louee");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);

            // Act
            var result = await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ResponseCode);
            Assert.Equal("Copie n'est pas disponible.", result.Message);
            _mockLocationRepository.Verify(r => r.AddLocation(It.IsAny<Location>()), Times.Never);
        }

        [Fact]
        public async Task LouerAsync_WithMaxLocationsReached_ReturnsErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(3);

            // Act
            var result = await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.ResponseCode);
            Assert.Contains("Votre forfait ne permet pas de louer plus de films", result.Message);
            Assert.Contains("Locations max: 3", result.Message);
            _mockLocationRepository.Verify(r => r.AddLocation(It.IsAny<Location>()), Times.Never);
        }

        [Fact]
        public async Task LouerAsync_WithUnavailableCopie_ReturnsErrorResponse()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Endommagé");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);

            // Act
            var result = await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ResponseCode);
            Assert.Equal("Aucune copie disponible pour ce film.", result.Message);
            _mockLocationRepository.Verify(r => r.AddLocation(It.IsAny<Location>()), Times.Never);
        }

        [Fact]
        public async Task LouerAsync_UpdatesCopieStatutToLouee()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>())).Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.Equal("Louee", copie.Statut);
        }

        [Fact]
        public async Task LouerAsync_WithForfaitHavingDureeMax_SetsDateRetourMax()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");
            Location? capturedLocation = null;

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>()))
                .Callback<Location>(loc => capturedLocation = loc)
                .Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(capturedLocation);
            Assert.NotNull(capturedLocation.DateRetourMax);
            Assert.True(capturedLocation.DateRetourMax > capturedLocation.DateDebut);
        }

        [Fact]
        public async Task LouerAsync_WithForfaitWithoutDureeMax_DateRetourMaxIsNull()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'B', 5, null);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");
            Location? capturedLocation = null;

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>()))
                .Callback<Location>(loc => capturedLocation = loc)
                .Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(capturedLocation);
            Assert.Null(capturedLocation.DateRetourMax);
        }

        [Fact]
        public async Task LouerAsync_CreatesLocationWithCorrectData()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");
            Location? capturedLocation = null;

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>()))
                .Callback<Location>(loc => capturedLocation = loc)
                .Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(capturedLocation);
            Assert.Equal(idUtilisateur, capturedLocation.IdUtilisateur);
            Assert.Equal(idCopie, capturedLocation.IdCopie);
            Assert.Null(capturedLocation.DateRetour);
            Assert.True((DateTime.Now - capturedLocation.DateDebut).TotalSeconds < 5);
        }

        [Fact]
        public async Task LouerAsync_WithClientNearingMaxLocations_AllowsRental()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(2);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>())).Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ResponseCode);
            _mockLocationRepository.Verify(r => r.AddLocation(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public async Task LouerAsync_BeginsAndCommitsTransaction()
        {
            // Arrange
            int idUtilisateur = 1;
            string idCopie = "COPY001";
            var client = CreateMockClient(idUtilisateur, 'A', 3, 7);
            var copie = CreateMockCopieFilm(idCopie, "Disponible");

            _mockLocationRepository.Setup(r => r.GetClientAvecForfait(idUtilisateur)).ReturnsAsync(client);
            _mockLocationRepository.Setup(r => r.GetCopie(idCopie)).ReturnsAsync(copie);
            _mockLocationRepository.Setup(r => r.CountLocationsActives(idUtilisateur)).ReturnsAsync(0);
            _mockLocationRepository.Setup(r => r.AddLocation(It.IsAny<Location>())).Returns(Task.CompletedTask);
            _mockLocationRepository.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            await _locationService.LouerAsync(idUtilisateur, idCopie);

            // Assert
            _mockDbContext.Verify(db => db.Database.BeginTransactionAsync(default), Times.Once);
            _mockTransaction.Verify(t => t.CommitAsync(default), Times.Once);
        }

        #endregion

        #region Helper Methods

        private Client CreateMockClient(int idUtilisateur, char codeForfait, int locationMax, int? dureeMax)
        {
            return new Client
            {
                IdUtilisateur = idUtilisateur,
                CodeForfait = codeForfait,
                Forfait = new Forfait
                {
                    Code = codeForfait,
                    NomForfait = $"Forfait {codeForfait}",
                    CoutMensuel = 9.99m,
                    LocationMax = locationMax,
                    DureeMax = dureeMax
                },
                Utilisateur = new Utilisateur
                {
                    IdUtilisateur = idUtilisateur,
                    Nom = "Test",
                    Prenom = "User",
                    Courriel = "test@example.com",
                    Telephone = "1234567890",
                    DateNaissance = new DateTime(1990, 1, 1),
                    MotDePasse = "password"
                }
            };
        }

        private CopieFilm CreateMockCopieFilm(string idCopie, string statut)
        {
            return new CopieFilm
            {
                IdCopie = idCopie,
                Statut = statut,
                IdFilm = 1
            };
        }

        #endregion
    }
}
