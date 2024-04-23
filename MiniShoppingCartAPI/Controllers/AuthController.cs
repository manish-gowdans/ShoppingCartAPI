using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShoppingCartAPI.Mapping_Code;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MiniShoppingCartAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IShoppingCartInfoRepository _shoppingCartInfoRepository;
        private readonly IConfiguration _configuration;
        

        public AuthController(IShoppingCartInfoRepository shoppingCartInfoRepository, IConfiguration configuration)
        {
            _shoppingCartInfoRepository = shoppingCartInfoRepository ?? throw new ArgumentNullException(nameof(shoppingCartInfoRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        private class AuthModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string UserType { get; set; }

            public AuthModel(Guid id,  string name, string email, string userType)
            {
                Id = id;   
                Name = name;
                Email = email;
                UserType = userType;
            }
        }

        public class UserInput
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class DisplayToUser
        {
            public Guid UserId { get; set; }

            public string AccessToken { get; set; }
            public string message { get; set; }
        }

        public class ErrorMessage
        {
            public string message { set; get; }
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="user">Fill in <br/> Username : "username" <br/> Password : "password" <br/> to obtain the access token and User Id </param>
        /// <returns>An IAction Result with</returns>
        /// <response code="200">Returns Ok Action when username and password matches from the given input</response>
        /// <response code="401">Returns Unauthorized Action when username does not exist or both username and password do not match</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> login(UserInput user)
        {

            var checkFromDb = await _shoppingCartInfoRepository.GetUserAsync(user.Username);

            if (checkFromDb == null)
            {
                return Unauthorized(new ErrorMessage { message = "Username does not exist please register" });
            }
            else if (checkFromDb.Password != user.Password)
            {
                return Unauthorized(new ErrorMessage { message = "Username and password does not match, Please retry!!!" });
            }
            else
            {
                var fromAuth = new AuthModel(checkFromDb.Uid, checkFromDb.Username, checkFromDb.EmailAddress, "customer");
                //encode security key
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

                //hash the security key
                var signingInCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //creating a list of claims for tokens
                var claims = new List<Claim>();
                claims.Add(new Claim("assigned_Id", fromAuth.Id.ToString()));
                claims.Add(new Claim("given_username", fromAuth.Name));
                claims.Add(new Claim("given_email", fromAuth.Email));
                claims.Add(new Claim("user_type", fromAuth.UserType));

                //create a token 
                var token = new JwtSecurityToken(_configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingInCredential);

                return Ok(new DisplayToUser
                {
                    UserId = _shoppingCartInfoRepository.GetUserIdFromDb(new Entities.User {  Username = user.Username, Password = user.Password}),
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    message = "Login Successful"
                });
            }
        }


    }
}
