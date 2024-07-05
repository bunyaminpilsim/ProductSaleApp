using Microsoft.AspNetCore.Mvc;
using ProductSaleApp.Data;
using ProductSaleApp.Models;

namespace ProductSaleApp.Repositories
{
    public class ProductRepository : IProductRepostory
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public void AddProduct(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                PurchasePrice = productDTO.PurchasePrice,
                SalePrice = productDTO.SalePrice,
                ImagePath = productDTO.ImagePath,
                Stock = productDTO.Stock
            };

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(ProductDTO productDTO)
        {
            var exProduct = _context.Products.FirstOrDefault(c => c.Id == productDTO.Id);
            if (exProduct != null)
            {
                _context.Products.Remove(exProduct);
                _context.SaveChanges();
            }
        }

        public List<ProductDTO> GetAllProducts()
        {
            var products = _context.Products.ToList();
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                productDTOs.Add(new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name??"",
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    ImagePath = product.ImagePath,
                    Stock = product.Stock
                });
            }

            return productDTOs;
        }

        public ProductDTO GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name ?? "",
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                ImagePath = product.ImagePath,
                Stock = product.Stock
            };
        }

        public void UpdateProduct(ProductDTO productDTO)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productDTO.Id);
            if (product != null)
            {
                product.Name = productDTO.Name;
                product.PurchasePrice = productDTO.PurchasePrice;
                product.SalePrice = productDTO.SalePrice;
                product.ImagePath = productDTO.ImagePath;
                product.Stock = productDTO.Stock;

                _context.SaveChanges();
            }
        }

        public void AddToCart(int productId)
        {
            // Sepete ekleme mantığını burada uygulayın.
            // Örneğin, Cart tablosuna ekleme yapabilirsiniz.
        }

        public void DecreaseStock(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var productDTOs = new List<ProductDTO>();
            if (product != null && product.Stock > 0)
            {

                productDTOs.Add(new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name ?? "",
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    ImagePath = product.ImagePath,
                    Stock = --product.Stock
                });

                _context.SaveChanges();
            }
        }

    }
}
