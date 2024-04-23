using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniShoppingCartAPI.DbContexts;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Mapping_Code;
using MiniShoppingCartAPI.Models;
using System.Linq;

namespace MiniShoppingCartAPI.Services
{
    public class ShoppingCartInfoRepository : IShoppingCartInfoRepository
    {
        #region Dependency Injection
        private readonly MiniShoppingCartApiContextSQLExpress _context;
      
        public ShoppingCartInfoRepository(MiniShoppingCartApiContextSQLExpress context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region For Users

        public void RegisterUsersAsync(User getUser)
        {
             _context.Users.Add(getUser);
             _context.SaveChanges();
            
        }

        public Guid GetUserIdFromDb(User user)
        {
            return _context.Users.Where(c => c.Username == user.Username && c.Password == user.Password).Select(c => c.Uid).FirstOrDefault();
        }

        public async Task<User> GetUserAsync(string username)
        {
            var result = await _context.Users.FirstOrDefaultAsync(c => c.Username == username);
            return result;
        }
            
        public async Task<User> GetUserAsync1(string username)
        {

            var resultList = from user in _context.Users
                             where user.Username == username
                             select user;
            return await resultList.FirstAsync();
        }

        public async Task<List<User>> GetUserAsync2(string username)
        {

            var resultList = from user in _context.Users
                where user.Username == username
                select user;
            return await resultList.ToListAsync();
        }

        public async Task<User> GetUserEmailAsync(string userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.EmailAddress == userEmail);

        }

        public async Task<User> GetUserFromIdAsync(Guid uid)
        {
            return await _context.Users.FirstAsync(c => c.Uid == uid);

        }
        #endregion

        #region For Products
        public async Task<IEnumerable<Product>> ViewProductsAsync()
        {
            return await _context.Products.OrderBy(c => c.Pid).ToListAsync();
        }
        public async Task<Product> ExtractProductAsync(Guid pid)
        {
            return await _context.Products.FirstAsync(p => p.Pid == pid);
        }
        #endregion

        #region For Cart
        public async Task<IEnumerable<Cart>> ViewCartAsync(Guid uid)
        {
            return await _context.Carts.Where(c=> c.UserId == uid).ToListAsync();
        }
        public bool CheckIfProductIsAddedIntoCart(Guid uid, Guid pid, Guid inputPid)
        {
            var productId = _context.Carts.Where(c => c.UserId == uid && c.ProductId == pid).Select(c => c.ProductId).FirstOrDefault();

            if (productId == inputPid)
            {
                return true;
            }

            return false;
        }
        public async Task AddToCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> CheckForCartIdExistsAsync(Guid uid, Guid pid)
        {
            var result = await _context.Carts.Where(c => c.ProductId == pid && c.UserId == uid).Select(c => c.CartId).FirstOrDefaultAsync();

            if(result == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    
      
        public async Task<int> GetQuantityOfEachProductAddedIntoCartAsync(Guid pid, Guid uid)
        {
            return await _context.Carts.Where(c => c.ProductId == pid && c.UserId == uid).Select(c => c.SetQuantity).FirstOrDefaultAsync();
        }
        
        public void RemoveFromCart(Guid uid, Guid pid)
        {
            var cid =_context.Carts.Where(c => c.UserId == uid && c.ProductId == pid).Select(c=> c.CartId).FirstOrDefault();
            
            if (cid != Guid.Empty)
            {
                //remove the cart Object
                _context.Carts.Remove(GetFromCart(cid));
                _context.SaveChanges();
            }
            //called in delete cart as well as in checkout
        }
        public Cart GetFromCart(Guid cid)
        {
            var getFromCart = _context.Carts.FirstOrDefault(c => c.CartId == cid);
            if (getFromCart == null)
            {
                throw new NullReferenceException(nameof(getFromCart));
            }

            return getFromCart;
        }

        public async Task<int> UpdateQuantityInCartAsync(Guid uid, Guid pid, int newQuantity)
        {
            var getCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == uid && c.ProductId == pid);
            if (getCart != null)
            {
                return getCart.SetQuantity += newQuantity;
            }
            return 0;

        }

        public async Task SaveUpdatedChanges()
        {
           await _context.SaveChangesAsync();
        }
        #endregion

        #region Checkout
        public int CheckIfProductsExistAfterCheckout(Guid uid, Guid pid)
        {
            return _context.Carts.Where(c => c.ProductId == pid && c.UserId == uid).Count();

        }
        #endregion

        #region Relation between Product & Cart
        public void UpdateQuanityOfProductsInDb(Guid pid, Guid uid, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Pid == pid);

            
          
            if (product != null)
            {
                if (product.Productname.IndexOf(' ') != -1)
                {
                    product.Productname = product.Productname.Substring(0, product.Productname.IndexOf(' '));
                    _context.SaveChanges();
                }
                int presentQuantity = product.Quantity;
                if (product.Quantity == 0)
                {
                    product.Productname = $"{product.Productname} - Out Of Stock";
                }

                //this line of code takes the decision of checking out the remaming quantity of products when the user added products in cart is more
                //else if(quantity > presentQuantity && presentQuantity != 0)
                //{
                //    UpdateQuantityOfEachProductInCartDb(pid, uid, quantity - product.Quantity);

                //    product.Productname = $"{product.Productname} - Out Of Stock";
                //    product.Quantity = 0;

                //}
                else if (presentQuantity == quantity)
                {
                    UpdateQuantityOfEachProductInCartDb(pid, uid, quantity - product.Quantity);
                    product.Quantity = presentQuantity - quantity;
                    RemoveFromCart(uid, pid);
                }
                else if(quantity < presentQuantity)
                {
                    product.Quantity = presentQuantity - quantity;
                    RemoveFromCart(uid, pid);
                }

                _context.SaveChanges();
            }

        }

        public void UpdateQuantityOfEachProductInCartDb(Guid pid, Guid uid, int updatedQuantity)
        {
            var fromCart = _context.Carts.FirstOrDefault(c=> c.UserId == uid && c.ProductId == pid);

            if (fromCart != null)
            {
               
                fromCart.SetQuantity = updatedQuantity;

                _context.SaveChanges();
            }
        }
        #endregion

     

    }
}
