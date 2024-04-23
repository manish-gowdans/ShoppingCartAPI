using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShoppingCartAPI.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CartId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public int SetQuantity { get; set; }

        public Cart()
        {

        }
    }
}
