using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;

namespace MiniShoppingCartAPI.Mapping_Code
{
    public class DtoToEntity
    {
        public static Cart FromCartDto(CartDto dto)
        {
            return new Cart
            {
   
                ProductId = dto.Pid,
                SetQuantity = dto.SetQuantity
            };
        }

        public static User FromUserDto(UserDto dto)
        {
            return new User(dto.Username, dto.EmailAddress, dto.Password);
        }
    }
}
