using Microsoft.EntityFrameworkCore;
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
        private readonly IMenuItemRepository _repository;

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
            var menuItemsQueryable = _testMenuItems.AsQueryable();
            _mockMenuItemSet = new Mock<DbSet<MenuItem>>();
            _mockMenuItemSet.As<IQueryable<MenuItem>>().Setup(m => m.Provider).Returns(menuItemsQueryable.Provider);
            _mockMenuItemSet.As<IQueryable<MenuItem>>().Setup(m => m.Expression).Returns(menuItemsQueryable.Expression);
            _mockMenuItemSet.As<IQueryable<MenuItem>>().Setup(m => m.ElementType).Returns(menuItemsQueryable.ElementType);
            _mockMenuItemSet.As<IQueryable<MenuItem>>().Setup(m => m.GetEnumerator()).Returns(menuItemsQueryable.GetEnumerator());

            // Setup Restaurant DbSet mock
            var restaurantsQueryable = _testRestaurants.AsQueryable();
            _mockRestaurantSet = new Mock<DbSet<Restaurant>>();
            _mockRestaurantSet.As<IQueryable<Restaurant>>().Setup(m => m.Provider).Returns(restaurantsQueryable.Provider);
            _mockRestaurantSet.As<IQueryable<Restaurant>>().Setup(m => m.Expression).Returns(restaurantsQueryable.Expression);
            _mockRestaurantSet.As<IQueryable<Restaurant>>().Setup(m => m.ElementType).Returns(restaurantsQueryable.ElementType);
            _mockRestaurantSet.As<IQueryable<Restaurant>>().Setup(m => m.GetEnumerator()).Returns(restaurantsQueryable.GetEnumerator());

            var reservationMenuItemsQueryable = _testReservationMenuItems.AsQueryable();
            _mockReservationMenuItemSet = new Mock<DbSet<ReservationMenuItem>>();
            _mockReservationMenuItemSet.As<IQueryable<ReservationMenuItem>>().Setup(m => m.Provider).Returns(reservationMenuItemsQueryable.Provider);
            _mockReservationMenuItemSet.As<IQueryable<ReservationMenuItem>>().Setup(m => m.Expression).Returns(reservationMenuItemsQueryable.Expression);
            _mockReservationMenuItemSet.As<IQueryable<ReservationMenuItem>>().Setup(m => m.ElementType).Returns(reservationMenuItemsQueryable.ElementType);
            _mockReservationMenuItemSet.As<IQueryable<ReservationMenuItem>>().Setup(m => m.GetEnumerator()).Returns(reservationMenuItemsQueryable.GetEnumerator());

            // Setup context
            var options = new DbContextOptions<AppDbContext>();
            _mockContext = new Mock<AppDbContext>(options);
            _mockContext.Setup(c => c.MenuItems).Returns(_mockMenuItemSet.Object);
            _mockContext.Setup(c => c.Restaurants).Returns(_mockRestaurantSet.Object);
            _mockContext.Setup(c => c.ReservationMenuItems).Returns(_mockReservationMenuItemSet.Object);

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

            _mockMenuItemSet.Setup(m => m.Add(It.IsAny<MenuItem>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _repository.CreateMenuItem(command);

            // Assert
            Assert.False(result.IsError);
            _mockMenuItemSet.Verify(m => m.Add(It.IsAny<MenuItem>()), Times.Once());
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

            _mockContext.Setup(m => m.Remove(It.IsAny<MenuItem>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

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

            var existingMenuItem = _testMenuItems.First();

            _mockContext.Setup(m => m.MenuItems.Update(It.IsAny<MenuItem>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

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