using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiniShoppingCartAPI.DbContexts;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading;
using System;
using System.Numerics;


namespace MiniShoppingCartAPI.nUnitTests.ServicesTests
{
    public class UserServicesTests
    {
        private ShoppingCartInfoRepository _repository;
        private Mock<MiniShoppingCartApiContextSQLExpress> _mockDbContext;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<MiniShoppingCartApiContextSQLExpress>();
            _repository = new ShoppingCartInfoRepository(_mockDbContext.Object);
        }

        private static DbSet<T> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return dbSet.Object;
        }


        [Test]
        public void GetUserIdFromDb_ReturnsGUID_ForValidUser()
        {
            // Arrange
            var user = new User { Username = "test-user", Password = "password" };
            var userId = Guid.Parse("44624b3a-b1b6-4f3b-dbc5-08dc3688f493");

            var users = new List<User>
            {
                new()
                {
                    Username = "test-user7", Password = "password",
                    Uid = Guid.Parse("fbd453ca-9e85-4dc4-305d-08dc35075d03")
                },
                new()
                {
                    Username = "test-user1", Password = "test-user1",
                    Uid = Guid.Parse("0808ab7c-cacb-4458-305e-08dc35075d03")
                },
                new()
                {
                    Username = "test-user2", Password = "test-user2",
                    Uid = Guid.Parse("83c5923a-8585-4980-305f-08dc35075d03")
                },
                new()
                {
                    Username = "test-user", Password = "password",
                    Uid = Guid.Parse("44624b3a-b1b6-4f3b-dbc5-08dc3688f493")
                }
            };

            _mockDbContext.Setup(c => c.Users).Returns(MockDbSet(users));

            // Act
            var result = _repository.GetUserIdFromDb(user);

            // Assert
            Assert.That(result, Is.EqualTo(userId));

        }

        [Test]
        public async Task GetUserAsync_ReturnUser_ForValidUsername()
        {
            //Arrange
            var user = new User { Username = "test-user", Password = "password" };
            var userId = Guid.Parse("44624b3a-b1b6-4f3b-dbc5-08dc3688f493");

            var users = new List<User>
            {
                new()
                {
                    Username = "test-user7", Password = "password",
                    Uid = Guid.Parse("fbd453ca-9e85-4dc4-305d-08dc35075d03")
                },
                new()
                {
                    Username = "test-user1", Password = "test-user1",
                    Uid = Guid.Parse("0808ab7c-cacb-4458-305e-08dc35075d03")
                },
                new()
                {
                    Username = "test-user2", Password = "test-user2",
                    Uid = Guid.Parse("83c5923a-8585-4980-305f-08dc35075d03")
                },
                new()
                {
                    Username = "test-user", Password = "password",
                    Uid = Guid.Parse("44624b3a-b1b6-4f3b-dbc5-08dc3688f493")
                }
            }.AsQueryable();

            var getFirstUser= users.FirstAsync();
            //Mock DbSet
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(users.AsQueryable().Provider);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.Expression)
                .Returns(users.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.ElementType)
                .Returns(users.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => users.GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<User>(users.Provider));

            //Mock DbContext
            _mockDbContext.Setup(d => d.Users).Returns(mockDbSet.Object);

            var result = await _repository.GetUserAsync2(user.Username);

        }
    }
}
