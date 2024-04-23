using NUnit.Framework;
using MiniShoppingCartAPI.Controllers;
using MiniShoppingCartAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Moq;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Services;

namespace MiniShoppingCartAPI.nUnitTests.ControllersTests
{
    
   public class UserControllerTests
   {
       private UserController _controller;
       private Mock<IShoppingCartInfoRepository> _repositoryMock;

       [SetUp]
       public void Setup()
       {
           _repositoryMock = new Mock<IShoppingCartInfoRepository>();
           _controller = new UserController(_repositoryMock.Object);
       }

       [Test]
       public async Task RegisterUsers_ReturnsCreated()
       {
           // Arrange
           var userDto = new UserDto("test-user","test-user@gmail.com","test-user");
           //check if the user matches with the following list of present users
           var mockUsers = new List<User>();
           mockUsers.Add(new User("test-user1", "test-user1@gmail.com", "test-user"));
           mockUsers.Add(new User("test-user2", "test-user2@example.com", "test-user"));
           mockUsers.Add(new User("test-user3", "test-user3@example.com", "test-user"));
           mockUsers.Add(new User("test-user4", "test-user4@example.com", "test-user"));

           foreach (var mockUser in mockUsers)
           {
               _repositoryMock.Setup(repo => repo.GetUserEmailAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
               _repositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

               // Act
               var result = await _controller.RegisterUsers(userDto);

               // Assert
               Assert.IsInstanceOf<CreatedAtActionResult>(result);
               var createdAtActionResult = result as CreatedAtActionResult;
               if (createdAtActionResult != null)
               {
                   Assert.That(createdAtActionResult.ActionName, Is.EqualTo("RegisterUsers"));
                   Assert.That(createdAtActionResult.Value, Is.EqualTo("User successfully registered, please login and obtain userId to continue shopping"));
               }
           }

            

       }

       [Test]
       public async Task RegisterUsers_ReturnsConflict_IfEmailExist()
       {
           // Arrange
           var userDto = new UserDto("test-user", "test-user@example.com", "test-user");
           var mockUser = new User("test-user", "test-user@example.com", "test-user");

           _repositoryMock.Setup(repo => repo.GetUserEmailAsync(userDto.EmailAddress)).ReturnsAsync(mockUser);

           // Act
           var result = await _controller.RegisterUsers(userDto);

           // Assert
           Assert.IsInstanceOf<ConflictObjectResult>(result);
           var conflictResult = result as ConflictObjectResult;
           Assert.AreEqual("User Email already exists please use different Email", conflictResult.Value);
       }

       [Test]
       public async Task RegisterUsers_ReturnsConflict_IfUsernameExist()
       {
           // Arrange
           var userDto = new UserDto("test-user", "test-user@gmail.com", "test-user");

           var mockUser = new User("test-user", "test-user@example.com", "test-user");

           _repositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

           // Act
           var result = await _controller.RegisterUsers(userDto);

           // Assert
           Assert.IsInstanceOf<ConflictObjectResult>(result);
           var conflictResult = result as ConflictObjectResult;
           if (conflictResult != null)
           {
               Assert.That(conflictResult.Value, Is.EqualTo("Username already exists please use different username"));
           }
       }
   }
}
