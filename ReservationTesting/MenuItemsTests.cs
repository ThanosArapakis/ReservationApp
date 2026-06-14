using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using ReservationApp.core.api.Application.Common.Interfaces.Restaurant;
using ReservationApp.core.api.Application.MenuItem.Commands.CreateMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.DeleteMenuItem;
using ReservationApp.core.api.Application.MenuItem.Commands.UpdateMenuItem;
using ReservationApp.core.api.Domain;
using ReservationApp.core.api.Infrastructure.Persistence;
using ReservationApp.core.api.Infrastructure.Persistence.Repository;
using Xunit;

namespace ReservationTesting
{
    public class MenuItemsTests
    {
        private readonly List<MenuItem> _testMenuItems;
        private readonly List<Restaurant> _testRestaurants;
        private readonly List<ReservationMenuItem> _testReservationMenuItems;
        private readonly Mock<DbSet<MenuItem>> _mockMenuItemSet;
        private readonly Mock<DbSet<Restaurant>> _mockRestaurantSet;
        private readonly Mock<DbSet<ReservationMenuItem>> _mockReservationMenuItemSet;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly MenuItemRepository _repository;

        public MenuItemsTests()
        {
            // Setup test data
            _testRestaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Test Restaurant" }
            };

            _testMenuItems = new List<MenuItem>
            {
                new MenuItem {
                    ItemId = 1,
                    Name = "Test Item 1",
                    Description = "Description 1",
                    Price = 9.99m,
                    Available = true,
                    CategoryId = 1,
                    RestaurantId = 1
                },
                new MenuItem {
                    ItemId = 2,
                    Name = "Test Item 2",
                    Description = "Description 2",
                    Price = 14.99m,
                    Available = true,
                    CategoryId = 2,
                    RestaurantId = 1
                }
            };

            _testReservationMenuItems = new List<ReservationMenuItem>
            {
                new ReservationMenuItem { MenuItemId = 1, ReservationId = 1, Quantity = 2 },
                new ReservationMenuItem { MenuItemId = 2, ReservationId = 1 }
            };



            // Setup MenuItem DbSet mock
            var menuItemsQueryable = _testMenuItems.AsQueryable().BuildMockDbSet();
            _mockMenuItemSet = new Mock<DbSet<MenuItem>>();

            // Setup Restaurant DbSet mock
            var restaurantsQueryable = _testRestaurants.AsQueryable().BuildMockDbSet();
            _mockRestaurantSet = new Mock<DbSet<Restaurant>>();

            var reservationMenuItemsQueryable = _testReservationMenuItems.AsQueryable().BuildMockDbSet();
            _mockReservationMenuItemSet = new Mock<DbSet<ReservationMenuItem>>();

            // Setup context
            var options = new DbContextOptions<AppDbContext>();
            _mockContext = new Mock<AppDbContext>(options);
            _mockContext.Setup(c => c.MenuItems).Returns(menuItemsQueryable.Object);
            _mockContext.Setup(c => c.Restaurants).Returns(restaurantsQueryable.Object);
            _mockContext.Setup(c => c.ReservationMenuItems).Returns(reservationMenuItemsQueryable.Object);

            _repository = new MenuItemRepository(_mockContext.Object);
        }

        [Fact]
        public async Task CreateMenuItem_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new CreateMenuItemCommand
            {
                Name = "New Item",
                Description = "New Description",
                Price = 19.99m,
                Available = true,
                CategoryId = 1,
                RestaurantId = 1
            };

            // Act
            var result = await _repository.CreateMenuItem(command);

            // Assert
            Assert.False(result.IsError);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CreateMenuItem_WithInvalidCategory_ReturnsError()
        {
            // Arrange
            var command = new CreateMenuItemCommand
            {
                Name = "New Item",
                Description = "New Description",
                Price = 19.99m,
                Available = true,
                CategoryId = 3, // Invalid category (should be 0-2)
                RestaurantId = 1
            };

            // Act
            var result = await _repository.CreateMenuItem(command);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains("Category has to be 0 - 2", result.FirstError.Description);
        }

        [Fact]
        public async Task Delete_ExistingMenuItem_ReturnsSuccess()
        {
            // Arrange
            var command = new DeleteMenuItemCommand { ItemId = 1 };

            // Act
            var result = await _repository.DeleteMenuItem(command);

            // Assert
            Assert.False(result.IsError);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateMenuItem_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new UpdateMenuItemCommand
            {
                ItemId = 1,
                Name = "Updated Item",
                Description = "Updated Description",
                Price = 24.99m,
                Available = true,
                CategoryId = 2,
                RestaurantId = 1
            };

            // Act
            var result = await _repository.UpdateMenuItemAsync(command);

            // Assert
            Assert.False(result.IsError);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateMenuItem_WithInvalidCategory_ReturnsError()
        {
            // Arrange
            var command = new UpdateMenuItemCommand
            {
                ItemId = 1,
                CategoryId = 3, // Invalid category
                RestaurantId = 1
            };

            // Act
            var result = await _repository.UpdateMenuItemAsync(command);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains("Category has to be 0 - 2", result.FirstError.Description);
        }

        [Fact]
        public async Task DeleteMenuItem_NonExistingItem_ReturnsError()
        {
            // Arrange
            var command = new DeleteMenuItemCommand { ItemId = 999 };

            // Act
            var result = await _repository.DeleteMenuItem(command);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains("MenuItem not found", result.FirstError.Description);
        }
    }
}