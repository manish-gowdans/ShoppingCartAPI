using System.ComponentModel.DataAnnotations;

namespace MiniShoppingCartAPI.Models
{
    public class ProductIdDto
    {
        [Required]
        public Guid Pid { get; set; }
    }
}
