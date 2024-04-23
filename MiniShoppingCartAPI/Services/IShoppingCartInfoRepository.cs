using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;

namespace MiniShoppingCartAPI.Services
{
    public interface IShoppingCartInfoRepository
    {

        void RegisterUsersAsync(User user);
        Guid GetUserIdFromDb(User user);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserAsync1(string username);
        Task<User> GetUserEmailAsync(string userEmail);
        Task<User> GetUserFromIdAsync(Guid uid);


        Task<IEnumerable<Product>> ViewProductsAsync();
        Task<Product> ExtractProductAsync(Guid pid);


        Task AddToCartAsync(Cart cart);
        Task<IEnumerable<Cart>> ViewCartAsync(Guid uid);
        Task<bool> CheckForCartIdExistsAsync(Guid uid, Guid pid);
        Task<int> GetQuantityOfEachProductAddedIntoCartAsync(Guid pid, Guid uid);
        void RemoveFromCart(Guid uid, Guid pid);
        void UpdateQuanityOfProductsInDb(Guid pid, Guid uid, int quantity);
        int CheckIfProductsExistAfterCheckout(Guid uid, Guid pid);
        bool CheckIfProductIsAddedIntoCart(Guid uid, Guid pid, Guid inputPid);
        Task<int> UpdateQuantityInCartAsync(Guid uid, Guid pid, int newQuantity);
        Task SaveUpdatedChanges();
    }
}
