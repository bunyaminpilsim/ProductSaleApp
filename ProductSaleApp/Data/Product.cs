using System.ComponentModel.DataAnnotations;

namespace ProductSaleApp.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public string? ImagePath { get; set; }
    }
}
