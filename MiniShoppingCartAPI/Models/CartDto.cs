using System.ComponentModel.DataAnnotations;

namespace MiniShoppingCartAPI.Models
{
    
    public class CartDto
    {
        
        [Required]
        public Guid Pid { get; set; }
        [Required]
        public int SetQuantity { get; set; }
        public CartDto() { }
        
        public CartDto(Guid pid, int setQuantity)
        {
            Pid = pid;
            SetQuantity = setQuantity;
        }
    }
}
