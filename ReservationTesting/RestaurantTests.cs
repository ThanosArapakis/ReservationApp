using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ReservationApp.core.api.Infrastructure.Persistence.Repository;
using ReservationApp.core.api.Domain;
using System.Collections.Generic;
using System.Linq;
using ReservationApp.core.api.Infrastructure.Persistence;
using ReservationApp.core.api.Application.Restaurant.Results;
using ErrorOr;
using System.Linq.Expressions;
using ReservationApp.core.api.Application.Restaurant.Commands;
using ReservationApp.core.api.Application.Restaurant.Queries;
using ReservationApp.core.api.Application.Restaurant.Queries.GetRestaurants;
using ReservationApp.core.api.Application.Restaurant.Commands.CreateRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.DeleteRestaurant;
using ReservationApp.core.api.Application.Restaurant.Commands.UpdateRestaurant;
using ReservationApp.core.api.Application.Common.Results;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using ReservationApp.core.api.Application.Common;
using MockQueryable.Moq;

namespace ReservationTesting
{
    public class RestaurantRepositoryTests
    {
        private readonly List<Restaurant> _testData;
        private readonly Mock<DbSet<Restaurant>> _mockSet;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly IRestaurantRepository _repository;
        private readonly Mock<ILogger<RestaurantRepository>> _mockLogger;

        public RestaurantRepositoryTests()
        {
            // Common test data setup
            _testData = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Restaurant A", Address = "Address A", PhoneNumber = "1234567890", Capacity = 50 ,
                MenuItems = new List<MenuItem>
                            {
                                new MenuItem { ItemId = 1, Name = "Item 1", Available = true, Price = 10.99m },
                                new MenuItem { ItemId = 2, Name = "Item 2", Available = false, Price = 15.99m }
                            }},
                new Restaurant { Id = 2, Name = "Restaurant B", Address = "Address B", PhoneNumber = "0987654321", Capacity = 75 ,
                MenuItems = new List<MenuItem>{ new MenuItem { ItemId = 3, Name = "Item 3", Available = true, Price = 12.99m } } },
            };

            _mockSet = _testData.AsQueryable().BuildMockDbSet();

            var options = new DbContextOptions<AppDbContext>();
            _mockContext = new Mock<AppDbContext>(options);
            _mockContext.Setup(c => c.Restaurants).Returns(_mockSet.Object);

            _mockLogger = new Mock<ILogger<RestaurantRepository>>();
            _repository = new RestaurantRepository(_mockContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllRestaurants_ReturnsAllRestaurants()
        {
            // Act
            ErrorOr<List<RestaurantResult>> result = await _repository.GetAllRestaurants();

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetRestaurant_WithValidName_ReturnsRestaurant()
        {
            // Arrange
            var query = new GetRestaurantsQuery { Name = "Restaurant B" };
            var filters = new List<Expression<Func<Restaurant, bool>>>();
            filters.Add(r => r.Name.Contains(query.Name));

            // Act
            var result = await _repository.GetRestaurant(query, filters);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(2, result.Value.RestaurantId);
        }

        [Fact]
        public async Task CreateRestaurant_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new CreateRestaurantCommand
            {
                Name = "New Restaurant",
                Address = "New Address",
                PhoneNumber = "1234567890",
                Capacity = 100
            };

            _mockSet.Setup(m => m.Add(It.IsAny<Restaurant>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _repository.CreateRestaurant(command);

            // Assert
            Assert.False(result.IsError);
            _mockSet.Verify(m => m.Add(It.IsAny<Restaurant>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteRestaurant_ExistingRestaurant_ReturnsSuccess()
        {
            // Arrange
            var restaurantId = 1;
            var restaurantToDelete = _testData.First(r => r.Id == restaurantId);
            var command = new DeleteRestaurantCommand { Id = restaurantId };

            // Setup test data for MenuItems and ReservationMenuItems
            var menuItemIds = restaurantToDelete.MenuItems.Select(mi => mi.ItemId).ToList();
            var testReservationMenuItems = new List<ReservationMenuItem>
            {
                new ReservationMenuItem { ReservationId = 1, MenuItemId = menuItemIds[0], Quantity = 2 },
                new ReservationMenuItem { ReservationId = 2, MenuItemId = menuItemIds[1], Quantity = 1 }
            };

            // Setup MenuItems DbSet mock using MockQueryable
            var mockMenuItemSet = restaurantToDelete.MenuItems.AsQueryable().BuildMockDbSet();

            // Setup ReservationMenuItems DbSet mock using MockQueryable
            var mockReservationMenuItemSet = testReservationMenuItems.AsQueryable().BuildMockDbSet();

            // Setup context mocks
            _mockContext.Setup(c => c.MenuItems).Returns(mockMenuItemSet.Object);
            _mockContext.Setup(c => c.ReservationMenuItems).Returns(mockReservationMenuItemSet.Object);

            // Setup FindAsync and Remove operations
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(restaurantToDelete);

            _mockContext.Setup(m => m.ReservationMenuItems.RemoveRange(It.IsAny<IEnumerable<ReservationMenuItem>>()));
            _mockSet.Setup(m => m.Remove(It.IsAny<Restaurant>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _repository.DeleteRestaurant(command);

            // Assert
            Assert.False(result.IsError);
            _mockContext.Verify(m => m.ReservationMenuItems.RemoveRange(It.IsAny<IEnumerable<ReservationMenuItem>>()), Times.Once());
            _mockSet.Verify(m => m.Remove(It.IsAny<Restaurant>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateRestaurant_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new UpdateRestaurantCommand
            {
                Id = 1,
                Name = "Updated Restaurant",
                Address = "Updated Address",
                PhoneNumber = "1234567890",
                Capacity = 150
            };

            _mockContext.Setup(m=> m.Restaurants.Update(It.IsAny<Restaurant>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _repository.UpdateRestaurantAsync(command);

            // Assert
            Assert.False(result.IsError);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetRestaurant_WithInvalidId_ReturnsError()
        {
            // Arrange
            var query = new GetRestaurantsQuery { RestaurantId = 999 };
            var filters = new List<Expression<Func<Restaurant, bool>>> 
            { 
                r => r.Id == query.RestaurantId 
            };

            // Act
            var result = await _repository.GetRestaurant(query, filters);

            // Assert
            Assert.True(result.IsError);
        }

        [Fact]
        public async Task DeleteRestaurant_NonExistingRestaurant_ReturnsError()
        {
            // Arrange
            var command = new DeleteRestaurantCommand { Id = 999 };

            // Act
            var result = await _repository.DeleteRestaurant(command);

            // Assert
            Assert.True(result.IsError);
        }
    }
}