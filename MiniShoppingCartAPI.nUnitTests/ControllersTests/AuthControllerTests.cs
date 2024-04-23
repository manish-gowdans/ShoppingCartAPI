using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniShoppingCartAPI.Controllers;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniShoppingCartAPI.Controllers.AuthController;

namespace MiniShoppingCartAPI.nUnitTests.ControllersTests
{
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<IShoppingCartInfoRepository> _Mockrepository;
        private Mock<IConfiguration> _Mockconfiguration;

        [SetUp]
        public void Setup()
        {
            
            _Mockrepository = new Mock<IShoppingCartInfoRepository>();
            _Mockconfiguration = new Mock<IConfiguration>();
            _authController = new AuthController(_Mockrepository.Object, _Mockconfiguration.Object);
        }

       

        [Test]
        public async Task login_ReturnUnauthorized_IfUserDoesNotExist()
        {
            //Arrange
            var userInput = new UserInput()
            {
                Username = "test-user",
                Password = "test-user"
 
            };

            var userNames = new List<string>();
            userNames.Add("test-user1");
            userNames.Add("test-user2");
            userNames.Add("test-user3");
            userNames.Add("test-user4");

            foreach(var user in userNames)
            {
                _Mockrepository.Setup(r => r.GetUserAsync(user)).ReturnsAsync(new Entities.User());

                //Act
                var result = await _authController.login(userInput);

                //Assert
                Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
                var unauthorizedResult = result as UnauthorizedObjectResult;

                Assert.That((unauthorizedResult.Value as ErrorMessage).message, Is.EqualTo("Username does not exist please register"));
                
            }
        }

        [Test]
        public async Task login_ReturnUnauthorized_IfUsernameAndPasswordDoesNotMatch()
        {
            //Arrange
            var userInput = new UserInput()
            {
                Username = "test-user",
                Password = "test-user"

            };

            var users = new List<User>{
                 new User { Username = "test-user", Password = "test-user1" },
                 new User { Username = "test-user2", Password = "test-user2" },
                 new User { Username = "test-user3", Password = "test-user3" }
            };


            _Mockrepository.Setup(r => r.GetUserAsync(It.IsAny<string>())).ReturnsAsync((string username) => users.FirstOrDefault(u => u.Username == userInput.Username));

                //Act
                var result = await _authController.login(userInput);

                //Assert
                Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
                var unauthorizedResult = result as UnauthorizedObjectResult;
                if (unauthorizedResult != null)
                {
                    Assert.That(((result as UnauthorizedObjectResult).Value as ErrorMessage).message, Is.EqualTo("Username and password does not match, Please retry!!!"));
                }
            
        }
    }
}
