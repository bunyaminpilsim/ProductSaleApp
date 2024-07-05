using Microsoft.AspNetCore.Mvc;
using ProductSaleApp.Models;
using ProductSaleApp.Repositories;

namespace ProductSaleApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepostory _productRepostorycs;
        private static List<ProductDTO> _selectedProducts = new List<ProductDTO>();

        public ProductController(IProductRepostory productRepostorycs)
        {
           _productRepostorycs = productRepostorycs;
        }

        [HttpGet]
        public IActionResult AddProduct()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductDTO productDTO )
        {
            if (ModelState.IsValid)
            {
                if (productDTO.File != null && productDTO.File.Length > 0)
                {
                    string extension = Path.GetExtension(productDTO.File.FileName);
                    string filename = Guid.NewGuid().ToString() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", filename);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        productDTO.File.CopyTo(stream);
                    }
                    productDTO.ImagePath = "/img/" + filename;
                }
                else
                {
                    ViewBag.Message = "Lütfen bir dosya seçin.";
                    return View(productDTO);
                }

           
                _productRepostorycs.AddProduct(productDTO);
                return RedirectToAction("ProductList");
            }
            return View(productDTO);
        }

        public IActionResult ProductList()
        {
            var product = _productRepostorycs.GetAllProducts();
            return View(product);
        }
        public IActionResult SaleList()
        {
            var product = _productRepostorycs.GetAllProducts();
            ViewBag.SelectedProducts = _selectedProducts;
            decimal totalEarn = _selectedProducts.Sum(p => p.SalePrice - p.PurchasePrice);
            decimal totalAmount = _selectedProducts.Sum(p => p.SalePrice);
            ViewBag.TotalAmount = totalAmount;
            ViewBag.TotalEarn = totalEarn;
            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(ProductDTO productDTO,int amount)
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {

            var product = _productRepostorycs.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _productRepostorycs.GetProductById(productDTO.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (productDTO.File != null && productDTO.File.Length > 0)
                {
                    // Eski dosyayı silme
                    if (!string.IsNullOrEmpty(existingProduct.ImagePath))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // Yeni dosyayı kaydetme
                    var fileExtension = Path.GetExtension(productDTO.File.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        productDTO.File.CopyTo(stream); // .Wait() yerine doğrudan senkron CopyTo kullanıyoruz
                    }
                    productDTO.ImagePath = "/img/" + fileName;
                }
                else
                {
                    productDTO.ImagePath = existingProduct.ImagePath; // Mevcut resmi koruyoruz
                }



                _productRepostorycs.UpdateProduct(productDTO);
                return RedirectToAction("ProductList");
            }
            return View(productDTO);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepostorycs.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepostorycs.DeleteProduct(product);
            return RedirectToAction("ProductList");
        }
        [HttpPost]
        [HttpPost]
        public IActionResult SelectProduct(int productId)
        {
            var product = _productRepostorycs.GetProductById(productId);
          
            if (product != null && product.Stock > 0)
            {
                _productRepostorycs.DecreaseStock(productId);

                _selectedProducts.Add(product); 
                return RedirectToAction("SaleList");
            }

            return RedirectToAction("SaleList", new { error = "Stok yetersiz." });
        }

    }
}
