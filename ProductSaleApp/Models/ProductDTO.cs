namespace ProductSaleApp.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? File { get; set; }

    }
}
