using ProductSaleApp.Models;

namespace ProductSaleApp.Repositories
{
    public interface IProductRepostory
    {
        void AddProduct(ProductDTO productDTO);
        void UpdateProduct(ProductDTO productDTO);
        void DeleteProduct(ProductDTO productDTO);
        void DecreaseStock(int id);
        void AddToCart(int id);
        List<ProductDTO> GetAllProducts();
        ProductDTO GetProductById(int id);
    }
}
