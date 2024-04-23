using MiniShoppingCartAPI.Models;

namespace MiniShoppingCartAPI.Services
{
    public interface IVerify
    {
        Task<bool> ValidUser(Guid userGuid);
        Task<bool> ProductExist(Guid productGuid);
        Task<bool> ProductQuantity(Guid productGuid);

    }
}
