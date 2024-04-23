using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Mapping_Code;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace MiniShoppingCartAPI.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        public IShoppingCartInfoRepository _repository;
        private readonly IVerify _verify;

        public CartController(IShoppingCartInfoRepository repository,IVerify verify)
        {
            _repository = repository?? throw new ArgumentNullException(nameof(repository));
            _verify = verify ?? throw new ArgumentNullException(nameof(verify));
        }

        public class Message()
        {
            public string? message { get; set;}
        }


        /// <summary>
        /// Add To Cart
        /// </summary>
        /// <param name="cartDto">Provide <br/> Pid : "Product Id" that needs to be added into the cart<br/> Set Quantity : "Quantity of the product added" <br/> NOTE: When the process is repeated for any added product, then the product quanitiy in cart is updated</param>
        /// <returns>An IAction Result with</returns>
        /// <response code="200">Returns Ok Action when : <br/> Product is added into cart <br/> When product is updated into cart </response>
        /// <response code="404">Returns Not Found Action when : <br/> User does not exist<br/>Product does not exist <br/> Product is out of stock </response>
        /// <response code="400">Returns Not Bad Request Action when : <br/> Product Quantity is set to zero<br/>Entered Product Quantity is more then the present stock </response>
        /// <response code="401">Returns Unauthorized Action when user is not authorized with access token</response>
        [HttpPost("cart/{Uid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddToCart(Guid Uid, CartDto cartDto)
        {
            var getProduct = await _repository.ExtractProductAsync(cartDto.Pid);

            if (!await _verify.ValidUser(Uid))
            {
                return NotFound(new Message { message = "Invalid User" });
            }
            else if (!await _verify.ProductExist(cartDto.Pid))
            {
                return NotFound(new Message { message = "Product does not exist in the inventory, please specify proper product Id!!!" });
            }
            else if (cartDto.SetQuantity == 0)
            {
                return BadRequest(new Message { message = "Quantity cannot be zero please enter the quantity of products" });
            }
            else if (!await _verify.ProductQuantity(cartDto.Pid))
            {
                return NotFound(new Message { message = "Product is out of stock" });
            }
            else if (getProduct.Quantity < cartDto.SetQuantity)
            {
                return BadRequest(new Message { message = $"Quantity of {getProduct.Productname} present in stock is : {getProduct.Quantity}, please update your cart!!!" });
            }

            else if (await _repository.CheckForCartIdExistsAsync(Uid, cartDto.Pid))
            {
                var updatedQaunity = await _repository.UpdateQuantityInCartAsync(Uid, cartDto.Pid, cartDto.SetQuantity);

                if (getProduct.Quantity < updatedQaunity)
                {
                    return BadRequest(new Message { message = $"Quantity of {getProduct.Productname} present in stock is : {getProduct.Quantity}, please update your cart!!!" });
                }
                else
                {
                    await _repository.SaveUpdatedChanges();
                    return Accepted(new Message { message = $"{getProduct.Productname} is updated to {updatedQaunity}" });
                }

            }

            //convert from dto to entity as AddToCart accepts entity
            var cartEntity = DtoToEntity.FromCartDto(cartDto);
            cartEntity.UserId = Uid;

            await _repository.AddToCartAsync(cartEntity);

            return Ok(new Message { message = $"{cartDto.SetQuantity} {getProduct.Productname} added into the cart" });

        }

        /// <summary>
        /// View Cart
        /// </summary>
        /// <returns>An IAction Result with</returns>
        /// <response code="200">Returns Ok Action for the user to view the products added into cart</response>
        /// <response code="404">Returns Not Found Action when : <br/> User does not exist<br/>User has not added any products into cart</response>
        /// <response code="401">Returns Unauthorized Action when user is not authorized with access token</response>
        [HttpGet("cart/{Uid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ViewCartDto>> ViewCart(Guid Uid)
        {
            if (!await _verify.ValidUser(Uid))
            {
                return NotFound(new Message { message = "Invalid User" });
            }

            var filteredCart = await _repository.ViewCartAsync(Uid);

            //check if user has added any products into the cart using list
            if (!filteredCart.Any())
            {
                return NotFound(new Message { message = "User has not added any products into the cart" });
            }
            //create a view cart list, in order to add the products from products database through user id that user has added into cart 
            var viewProductList = new List<ViewCartInfo>();

            foreach (var item in filteredCart)
            {
                //get single product with the matched pid for the specific user
                var getSingleProduct = await _repository.ExtractProductAsync(item.ProductId);

                //get quantity of each product added into the cart by user and add it into retrieving list
                var quantityOfEachProduct = await _repository.GetQuantityOfEachProductAddedIntoCartAsync(item.ProductId, Uid);

                //add all the extracted products into the list
                ExtractProduct.AddEverySingleProductFromCartToList(getSingleProduct, viewProductList, quantityOfEachProduct);

            }

            //return the ViewCartDto by getting username from given user id and total quantity of the products
            //added into cart and quantity of each product added into cart
            return Ok(new ViewCartDto((await _repository.GetUserFromIdAsync(Uid)).Username,filteredCart.Count(), viewProductList));
        }

        /// <summary>
        /// Remove From Cart
        /// </summary>
        /// <param name="pid">Provide <br/> Pid : "Product Id" that needs to be removed into the cart<br/> </param>
        /// <returns>An IAction Result with</returns>
        /// <response code="200">Returns Ok Action when product is removed from cart </response>
        /// <response code="404">Returns Not Found Action when : <br/> User does not exist<br/>Product does not exist <br/> User has not added any products into cart<br/> Product is out of stock </response>
        /// <response code="401">Returns Unauthorized Action when user is not authorized with access token</response>
        [HttpDelete("cart/{Uid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RemoveFromCart(Guid Uid, ProductIdDto pid)
        {
            if (!await _verify.ValidUser(Uid))
            {
                return NotFound(new Message { message = "Invalid User" });
            }
            if(!await _verify.ProductExist(pid.Pid))
            {
                return NotFound(new Message { message = "Product does not exist in the inventory, please specify proper product Id!!!" });
            }
            //then check if user has added any products into cart
            var filteredCart = await _repository.ViewCartAsync(Uid);
            if (!filteredCart.Any())
            {
                return NotFound(new Message { message = "User has not added any products into the cart" });
            }

            foreach (var item in filteredCart)
            {
                //check if user has added the specified product id inside cart
                //this reduces the time in checking for each and every Pid
                if(_repository.CheckIfProductIsAddedIntoCart(Uid, item.ProductId, pid.Pid))
                {
                    _repository.RemoveFromCart(Uid, pid.Pid);
                    var getProductName = await _repository.ExtractProductAsync(pid.Pid);
                    return Ok(new Message { message = $"{getProductName.Productname} removed from cart" });
                }
                
            }

            return NotFound(new Message { message = "Product does not exist in the cart" });
        }

        /// <summary>
        /// Checkout From Cart
        /// </summary>
        /// <returns>An IAction Result with</returns>
        /// <response code="200">Returns Ok Action when products from user cart is checked out and Some products are checked out whereas others are left due to less stock </response>
        /// <response code="404">Returns Not Found Action when : <br/> User does not exist<br/>Product does not exist <br/> User has not added any products into cart</response>
        /// <response code="401">Returns Unauthorized Action when user is not authorized with access token</response>
        [HttpPost("checkout/{Uid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Checkout(Guid Uid)
        {
            if (!await _verify.ValidUser(Uid))
            {
                return NotFound(new Message { message = "Invalid User" });
            }

            //then check if user has added any products into cart
            var filteredCart = await _repository.ViewCartAsync(Uid);
            if (filteredCart.Count() == 0)
            {
                return NotFound(new Message { message = "User has not added any products into the cart" });
            }
            var remainingProductsInCart = 0;
            foreach (var item in filteredCart)
            {
                var quantityOfEachProduct = await _repository.GetQuantityOfEachProductAddedIntoCartAsync(item.ProductId, Uid);

                _repository.UpdateQuanityOfProductsInDb(item.ProductId,Uid, quantityOfEachProduct);



                remainingProductsInCart += _repository.CheckIfProductsExistAfterCheckout(Uid, item.ProductId);
            }

            //if(remainingProductsInCart != 0)
            //{
            //    return Ok("Due to less/unavailability of the products in stock only the remaining possible products were checked out\n" +
            //        $"Hence, {remainingProductsInCart} number of products in your cart are left out \n" +
            //        $"Please check your cart to verify ");
            //}
            if (remainingProductsInCart == 1)
            {
                return Ok(new Message
                {
                    message = $"Not all products were checked out due to unavailability or not enough quantity present in stock, please check your cart as {remainingProductsInCart} number of product is left out"
                });
            }
            if (remainingProductsInCart > 1)
            {
                return Ok(new Message
                {
                    message = $"Not all products were checked out due to unavailability or not enough quantity present in stock, please check your cart as {remainingProductsInCart} number of products are left out"
                });
            }



            return Ok(new Message { message = $"Your order has been confirmed and all the products in cart are checked out" });
        }
    }
}
