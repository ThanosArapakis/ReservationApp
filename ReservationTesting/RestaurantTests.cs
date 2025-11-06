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

namespace ReservationTesting
{
        public class RestaurantRepositoryTests
        {
            [Fact]
            public async Task GetAll_ReturnsAllRestaurants() // Changed method to async
            {
                var data = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "A" },
                new Restaurant { Id = 2, Name = "B" }
            }.AsQueryable();

                var mockSet = new Mock<DbSet<Restaurant>>();
                mockSet.As<IQueryable<Restaurant>>().Setup(m => m.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<Restaurant>>().Setup(m => m.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<Restaurant>>().Setup(m => m.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<Restaurant>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator()); 

                var mockContext = new Mock<AppDbContext>();
                mockContext.Setup(c => c.Restaurants).Returns(mockSet.Object);

                var repo = new RestaurantRepository(mockContext.Object);
                var result = await repo.GetAllRestaurants(); // Await the Task and fix the type
             
                Assert.True(result.IsError == false); // Ensure no errors occurred
                Assert.Equal(2, result.Value.Count); // Access the Value property of ErrorOr
            }

        }
}