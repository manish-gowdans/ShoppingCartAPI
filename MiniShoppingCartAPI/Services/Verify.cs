using Microsoft.AspNetCore.Mvc;
using MiniShoppingCartAPI.Models;
using static MiniShoppingCartAPI.Controllers.CartController;

namespace MiniShoppingCartAPI.Services
{
    public class Verify:IVerify
    {
        public IShoppingCartInfoRepository _repository;

        public Verify(IShoppingCartInfoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public virtual async Task<bool> ValidUser(Guid userGuid)
        {
            var checkUser = await _repository.GetUserFromIdAsync(userGuid);
            if (checkUser == null)
            {
                return false;
            }
            return true;
        }

        public virtual async Task<bool> ProductExist(Guid productGuid)
        {
            var getProduct = await _repository.ExtractProductAsync(productGuid);
            if (getProduct == null)
            {
                return false;
            }
            return true;
        }

        public virtual async Task<bool> ProductQuantity(Guid productGuid)
        {
            //Check if Product is Out Of Stock
            var checkForProductQuantity = await _repository.ExtractProductAsync(productGuid);
            if (checkForProductQuantity.Quantity == 0)
            {
                return false;
            }
            return true;

        }

        
    }
}
