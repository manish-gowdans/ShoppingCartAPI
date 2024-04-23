using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShoppingCartAPI.DbContexts;
using MiniShoppingCartAPI.Mapping_Code;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;

namespace MiniShoppingCartAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IShoppingCartInfoRepository _shoppingCartInfoRepository;

        public ProductController(IShoppingCartInfoRepository shoppingCartInfoRepository)
        {
            _shoppingCartInfoRepository = shoppingCartInfoRepository ?? throw new ArgumentNullException(nameof(shoppingCartInfoRepository));
        }


        /// <summary>
        /// Product Listing
        /// </summary>
        /// <response code="200">Returns Ok Action when products are present in database and lists them for users</response>
        /// <response code="404">Returns Not Found Action when products database is empty </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ViewProducts()
        {
            var viewProducts = await _shoppingCartInfoRepository.ViewProductsAsync();

            if (viewProducts.Count() == 0)
            {
                return NotFound("The products list are empty!!! Please check after some time");
            }
            var productList = new List<ProductDto>();
           
              foreach (var product in viewProducts)
                {
                    productList.Add(new ProductDto
                    {
                        Pid = product.Pid,
                        Productname = product.Productname,
                        Quantity = product.Quantity,
                        Price = product.Price
                    });
                }

              return Ok(productList);

        }
    }
}
