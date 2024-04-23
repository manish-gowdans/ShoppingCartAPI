namespace MiniShoppingCartAPI.Models
{
    public class ViewCartDto
    {

        public string Username { get; set; }
        public int TotalProductsAddedIntoCart { get; set; }

        public List<ViewCartInfo> viewCart { get; set; }

        public ViewCartDto(string username, int totalProductsAddedIntoCart, List<ViewCartInfo> viewCart)
        {
            Username = username;
            TotalProductsAddedIntoCart= totalProductsAddedIntoCart;
            this.viewCart = viewCart;
        }
    }
}
