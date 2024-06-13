using Microsoft.AspNetCore.Mvc;
using GestiuneaStocului.Models;
using Microsoft.EntityFrameworkCore;

namespace GestiuneaStocului.Controllers;

public class ProductsController : Controller {
    private readonly ILogger<ProductsController> _logger;
    private readonly GestiuneaStoculuiDbContext _context;

    public ProductsController(ILogger<ProductsController> logger, GestiuneaStoculuiDbContext context) {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IActionResult AddProduct() {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct([Bind("Name", "Stock", "Price")] Products product) {
        if (ModelState.IsValid) {
            var existProduct = _context.Products.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
            if (existProduct != null) {
                ViewBag.Message = $"The product '{product.Name}' is already in stock. Do you want to ";
                return View(product);
            } else {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                var report = new ProductReports {
                    ProductId = product.ProductId,
                    QuantityIn = product.Stock,
                    QuantityOut = 0,
                    Stock = product.Stock,
                    Product = product
                };
                _context.Add(report);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "The product has been added.";
            }
        }
        return RedirectToAction("AddProduct");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStock([Bind("Name", "Stock")] Products products) {
        var existProduct = _context.Products.FirstOrDefault(p => p.Name.ToLower() == products.Name.ToLower());
        if (existProduct != null) {
            existProduct.Stock += products.Stock;
            _context.Update(existProduct);
            await _context.SaveChangesAsync();
            var report = new ProductReports {
                    ProductId = existProduct.ProductId,
                    QuantityIn = products.Stock,
                    QuantityOut = 0,
                    Stock = existProduct.Stock,
                    Product = existProduct
            };
            _context.Add(report);
            await _context.SaveChangesAsync();
            ViewBag.Message = $"The stock for '{products.Name}' has been updated successfully!";
            ViewBag.Success = true;
        } else {
            ViewBag.Message = $"The product '{products.Name}' is not in stock.";
            ViewBag.Success = false;
        }
        return View(products);
    }

    public IActionResult UpdateStock() {
        return View();
    }

    public IActionResult Detail() {
        var products = _context.Products.OrderBy(p => p.ProductId).ToList();
        return View(products);
    }

    public IActionResult ProductWithdrawal() {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductWithdrawal(string ProductName, int Quantity) {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == ProductName.ToLower());
        if (product == null) {
            ViewBag.ErrorMessage = "Product not found.";
            return View();
        }
        if (product.Stock < Quantity) {
            ViewBag.ErrorMessage = "Not enough stock available.";
            return View();
        }
        product.Stock -= Quantity;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        var report = new ProductReports {
            ProductId = product.ProductId,
            QuantityIn = 0,
            QuantityOut = Quantity,
            Stock = product.Stock,
            Product = product
        };
        _context.Add(report);
        await _context.SaveChangesAsync();
        ViewBag.SuccessMessage = "Product quantity updated successfully.";
        return View();
    }
}