namespace MiniShoppingCartAPI.Models
{
    public class ViewCartInfo
    {

        public Guid ProductId { get; set; }

        public String ProductName { get; set; }

        public int ProductQuantityPresentInUsersCart { get; set; }
        //public int ProductPrice { get; set; }


        public ViewCartInfo(String productName)
        {
            ProductName = productName;
        }
        
       

    }
}
