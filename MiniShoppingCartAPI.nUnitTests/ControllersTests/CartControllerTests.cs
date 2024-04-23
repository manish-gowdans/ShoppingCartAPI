using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MiniShoppingCartAPI.Controllers;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;
using Moq;

namespace MiniShoppingCartAPI.nUnitTests.ControllersTests
{
    public class CartControllerTests
    {
        private CartController _controller;
        private Mock<IShoppingCartInfoRepository> _mockRepository;
        private Mock<IVerify> _mockVerify;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IShoppingCartInfoRepository>();
            _mockVerify = new Mock<IVerify>();
            _controller = new CartController(_mockRepository.Object, _mockVerify.Object);
        }

        [Test]
        public async Task AddToCart_ReturnOk_WhenValidUserAddsValidProductAndValidQuantityToCartForFirstTime()
        {
            //Arrange
            _mockVerify.Setup(c => c.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(c => c.ProductExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(c => c.ProductQuantity(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product(It.IsAny<string>(), 11, It.IsAny<int>()));
            _mockRepository.Setup(r => r.CheckForCartIdExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            //Act
            var addToCart = await _controller.AddToCart(It.IsAny<Guid>(), new CartDto(It.IsAny<Guid>(), 10));

            //Assert
            Assert.That(addToCart, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task AddToCart_ReturnAccepted_IfUserPreviouslyAddedTheProductInCart()
        {
            //Arrange
            _mockVerify.Setup(c => c.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(c => c.ProductExist(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(c => c.ProductQuantity(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product(It.IsAny<string>(), 11, It.IsAny<int>()));
            _mockRepository.Setup(r => r.CheckForCartIdExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            var addToCart = await _controller.AddToCart(It.IsAny<Guid>(), new CartDto(It.IsAny<Guid>(), 10));

            //Assert
            Assert.That(addToCart, Is.TypeOf<AcceptedResult>());
        }

        [Test]
        public async Task ViewCart_ReturnNotFound_IfValidUserCartIsEmpty()
        {
            //Arrange
            _mockVerify.Setup(c => c.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>());

            //Act
            var viewCart = await _controller.ViewCart(It.IsAny<Guid>());

            //Assert
            Assert.That(viewCart.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task ViewCart_ReturnOk_ForValidUsersCart()
        {
            //Arrange
            _mockVerify.Setup(c => c.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>()
            {
                new()
                {
                    CartId = It.IsAny<Guid>(),
                    ProductId = It.IsAny<Guid>(),
                    UserId = It.IsAny<Guid>(),
                    SetQuantity = It.IsAny<int>()
                }
            });
            _mockRepository.Setup(r => r.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            //_mockRepository.Setup(r => r.GetQuantityOfEachProductAddedIntoCartAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(It.IsAny<int>());
            _mockRepository.Setup(r => r.GetUserFromIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User());

            //Act
            var viewCart = await _controller.ViewCart(It.IsAny<Guid>());

            //Assert
            //Assert.IsNotEmpty(viewCart.Value.viewCart);
            Assert.That(viewCart.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task ViewCart__ReturnNotFound_IfUserHasAddedZeroProducts()
        {
            _mockVerify.Setup(c => c.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>());

            var viewCart = await _controller.ViewCart(It.IsAny<Guid>());

            Assert.That(viewCart.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task RemoveFromCart_ReturnOk_IfSuccessfullyRemoved()
        {
            _mockVerify.Setup(v => v.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(v => v.ProductExist(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>()
            {
                new()
                {
                    CartId = It.IsAny<Guid>(),
                    ProductId = It.IsAny<Guid>(),
                    UserId = It.IsAny<Guid>(),
                    SetQuantity = It.IsAny<int>()
                }
            });
            _mockRepository
                .Setup(m => m.CheckIfProductIsAddedIntoCart(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(true);
            _mockRepository
                .Setup(m => m.ExtractProductAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());

            var result = await _controller.RemoveFromCart(It.IsAny<Guid>(), new ProductIdDto() { Pid = It.IsAny<Guid>() });

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task RemoveFromCart_ReturnNotFound_IfUserHasAddedZeroProducts()
        {
            _mockVerify.Setup(v => v.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(v => v.ProductExist(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>());

            var result = await _controller.RemoveFromCart(It.IsAny<Guid>(), new ProductIdDto() { Pid = It.IsAny<Guid>() });

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task RemoveFromCart_ReturnNotFound_IfProductDoesNotExist_Or_UserGaveWrongPidToRemove()
        {
            _mockVerify.Setup(v => v.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockVerify.Setup(v => v.ProductExist(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>()
            {
                new()
                {
                    CartId = It.IsAny<Guid>(),
                    ProductId = It.IsAny<Guid>(),
                    UserId = It.IsAny<Guid>(),
                    SetQuantity = It.IsAny<int>()
                }
            });
            _mockRepository
                .Setup(m => m.CheckIfProductIsAddedIntoCart(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(false);


            var result = await _controller.RemoveFromCart(It.IsAny<Guid>(), new ProductIdDto() { Pid = It.IsAny<Guid>() });

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var getObject = result as NotFoundObjectResult;
            Assert.IsNotNull(getObject);
            var getMessageObject = getObject.Value;
            var mes = getMessageObject as CartController.Message;
            Assert.That(mes.message, Is.EqualTo("Product does not exist in the cart"));


        }

        [Test]
        public async Task Checkout_ReturnOk_IfAllProductsAreCheckedOut()
        { 
            _mockVerify.Setup(v => v.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>()
            {
                new()
                {
                    CartId = It.IsAny<Guid>(),
                    ProductId = It.IsAny<Guid>(),
                    UserId = It.IsAny<Guid>(),
                    SetQuantity = It.IsAny<int>()
                }
            });
            _mockRepository
                .Setup(m => m.GetQuantityOfEachProductAddedIntoCartAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>());

            var result = await _controller.Checkout(It.IsAny<Guid>());

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Checkout_ReturnOk_IfAllProductsAreNotCheckedOut()
        {
            _mockVerify.Setup(v => v.ValidUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockRepository.Setup(r => r.ViewCartAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Cart>()
            {
                new()
                {
                    CartId = It.IsAny<Guid>(),
                    ProductId = It.IsAny<Guid>(),
                    UserId = It.IsAny<Guid>(),
                    SetQuantity = It.IsAny<int>()
                }
            });
            _mockRepository
                .Setup(m => m.GetQuantityOfEachProductAddedIntoCartAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>());

            _mockRepository
                .Setup(m => m.CheckIfProductsExistAfterCheckout(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(2);

            var result = await _controller.Checkout(It.IsAny<Guid>());

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var getObject = result as OkObjectResult;
            Assert.IsNotNull(getObject);
            var getMessageObject = getObject.Value;
            var mes = getMessageObject as CartController.Message;
            Assert.That(mes.message, Is.EqualTo($"Not all products were checked out due to unavailability or not enough quantity present in stock, please check your cart as 2 number of products are left out"));

        }
    }
}
