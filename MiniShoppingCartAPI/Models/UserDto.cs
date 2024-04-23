using MiniShoppingCartAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MiniShoppingCartAPI.Models
{
    public class UserDto
    {
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        [EmailAddress]
        [MaxLength(30)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        public UserDto(string username, string emailAddress, string password)
        {
            Username = username; 
            EmailAddress = emailAddress;
            Password = password;
        }

        public static UserDto FromEntity(User entity)
        {
            return new UserDto(entity.Username, entity.EmailAddress, entity.Password);
        }
    }
}
