using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Services;
using Moq;

namespace MiniShoppingCartAPI.nUnitTests.ServicesTests
{
    public class VerifyTests
    {
        private Verify _verify;
        private Mock<IShoppingCartInfoRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IShoppingCartInfoRepository>();
            _verify = new Verify(_mockRepository.Object);
        }

        [Test]
        public async Task ValidUser_ReturnFalse_IfUserNotFound()
        {
            //Arrange
            _mockRepository.Setup(r => r.GetUserFromIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            //Act
            var getResult = await _verify.ValidUser(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.False);
        }

        [Test]
        public async Task ValidUser_ReturnTrue_IfUserFound()
        {
            //Arrange
            _mockRepository.Setup(r => r.GetUserFromIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User());

            //Act
            var getResult = await _verify.ValidUser(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.True);
        }

        [Test]
        public async Task ProductExist_ReturnFalse_IfProductNotFound()
        {
            //Arrange
            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            //Act
            var getResult = await _verify.ProductExist(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.False);
        }

        [Test]
        public async Task ProductExist_ReturnTrue_IfProductFound()
        {
            //Arrange
            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            //Act
            var getResult = await _verify.ProductExist(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.True);
        }

        [Test]
        public async Task ProductQuantity_ReturnFalse_IfProductQuantityIsZero()
        {
            //Arrange
            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            //Act
            var getResult = await _verify.ProductQuantity(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.False);
        }

        [Test]
        public async Task ProductQuantity_ReturnTrue_IfProductQuantityIsNotZero()
        {
            //Arrange
            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product()
            {
                Quantity = 1
            });

            //Act
            var getResult = await _verify.ProductQuantity(It.IsAny<Guid>());

            //Assert
            Assert.That(getResult, Is.True);
        }
    }
}
