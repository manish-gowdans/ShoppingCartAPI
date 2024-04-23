using MiniShoppingCartAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShoppingCartAPI.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uid { get; set; }
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }

        [EmailAddress]
        [MaxLength(30)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }

        public User() { }

        public User(string username, string emailAddress, string password)
        {
            Username = username;
            EmailAddress = emailAddress;
            Password = password;
        }



    }
}
