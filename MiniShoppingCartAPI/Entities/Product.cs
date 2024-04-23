using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShoppingCartAPI.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Pid { get; set; }
        [Required]
        [MaxLength(25)]
        public string Productname { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        public Product() { }
        public Product(string productname, int quantity, int price)
        {
            Productname = productname;
            Quantity = quantity;
            Price = price;
        }
    }
}
