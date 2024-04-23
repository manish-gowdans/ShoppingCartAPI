using MiniShoppingCartAPI.Entities;
using MiniShoppingCartAPI.Models;
using MiniShoppingCartAPI.Services;

namespace MiniShoppingCartAPI.Mapping_Code
{
    public class ExtractProduct
    {


        public static List<ViewCartInfo> AddEverySingleProductFromCartToList(Product getSingleProduct, List<ViewCartInfo> viewProductList, int quantityOfEachProduct)
        {
 
                viewProductList.Add(new ViewCartInfo(getSingleProduct.Productname)
                {
                    ProductId = getSingleProduct.Pid,
                    ProductQuantityPresentInUsersCart = quantityOfEachProduct
                    //ProductPrice = item.Price
                });
            

            return viewProductList;
        }


        
    }
}
