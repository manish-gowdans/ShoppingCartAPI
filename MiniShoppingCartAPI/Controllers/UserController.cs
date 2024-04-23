using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Mapping_Code;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;

namespace MiniShoppingCartAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IShoppingCartInfoRepository _shoppingCartInfoRepository;

        

        public UserController(IShoppingCartInfoRepository shoppingCartInfoRepository)
        {
            _shoppingCartInfoRepository = shoppingCartInfoRepository ?? throw new ArgumentNullException(nameof(shoppingCartInfoRepository));
            
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="userDto">Fill in <br/> Username : "username" <br/> Email : "email@gmail.com" <br/> Password : "password" <br/> for successful registration</param>
        /// <returns>An IAction Result with</returns>
        /// <response code="201">Returns Created Action when user is successfully registered</response>
        /// <response code="409">Returns Conflict Action when username or userEmail exist in database</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUsers(UserDto userDto)
        {
            var addUsers = DtoToEntity.FromUserDto(userDto);
           
                var checkforUserEmail = await _shoppingCartInfoRepository.GetUserEmailAsync(addUsers.EmailAddress);
                if (checkforUserEmail != null && checkforUserEmail.EmailAddress == userDto.EmailAddress)
                {
                    return Conflict("User Email already exists please use different Email");
                }
                var checkforUsername = await _shoppingCartInfoRepository.GetUserAsync(addUsers.Username);
                if (checkforUsername != null && checkforUsername.Username == userDto.Username)
                {
                    return Conflict("Username already exists please use different username");
                }
                else
                {
                    _shoppingCartInfoRepository.RegisterUsersAsync(addUsers);
                    return CreatedAtAction(nameof(RegisterUsers), "User successfully registered, please login and obtain userId to continue shopping");
                }

        }

        #region Login without Auth
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserDto userDto)
        //{
        //    var userEntity = DtoToEntity.FromUserDto(userDto);
        //    var checkFromDb = await _shoppingCartInfoRepository.GetUserAsync(userEntity.Username);

        //    if(checkFromDb == null)
        //    {
        //        return Unauthorized("Username does not exist please register");
        //    }
        //    else if (checkFromDb.Password != userDto.Password)
        //    {
        //        return Unauthorized("Username and password does not match, Please retry!!!");
        //    }
        //    else
        //    {
        //        return Ok($"Login Successful, please use your Id : {_shoppingCartInfoRepository.GetUserIdFromDb(userEntity)} to start shopping");
        //    }
        //}
        #endregion
    }
}
