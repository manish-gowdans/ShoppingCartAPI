using System.ComponentModel.DataAnnotations;

namespace MiniShoppingCartAPI.Models
{
    public class ProductDto
    {
        [Required]
        public Guid Pid {  get; set; }
        [Required]
        [MaxLength(25)]
        public string? Productname { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        public int Price { get; set; }

    }
}
