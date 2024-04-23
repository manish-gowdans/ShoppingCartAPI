using MiniShoppingCartAPI.DbContexts;
using MiniShoppingCartAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiniShoppingCartAPI.nUnitTests.ServicesTests
{
    internal class ProductServicesTests
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
    }
}
