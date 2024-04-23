using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniShoppingCartAPI.Controllers;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShoppingCartAPI.nUnitTests.ControllersTests
{
    public class ProductControllerTests
    {
        private ProductController _controller;
        private Mock<IShoppingCartInfoRepository> _Mockrepository;

        [SetUp]
        public void Setup()
        {
              _Mockrepository = new Mock<IShoppingCartInfoRepository>();
            _controller = new ProductController(_Mockrepository.Object);
        }

        [Test]
        public async Task ViewProducts_ReturnNotFound_IfProductStockIsEmpty()
        {
            //Arrange
            _Mockrepository.Setup(r => r.ViewProductsAsync()).ReturnsAsync(new List<Product>());

            //Act
            var result = await _controller.ViewProducts();

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            
            Assert.That(notFoundResult.Value, Is.EqualTo("The products list are empty!!! Please check after some time"));

        }

        [Test]
        public async Task ViewProducts_ReturnOk_IfProductStockIsNotEmpty()
        {
            //Arrange
            _Mockrepository.Setup(r => r.ViewProductsAsync()).ReturnsAsync(new List<Product>()
            {
                new Product("Apple", 10, 100)
               {
                 Pid = Guid.NewGuid()
               }
            });

            //Act
            var result = await _controller.ViewProducts();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var productsFoundResult = result as OkObjectResult;

            Assert.That(productsFoundResult, Is.TypeOf<OkObjectResult>());//to check if action result matches

        }
    }
}
