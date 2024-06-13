using GestiuneaStocului.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestiuneaStocului.Controllers;

public class ReportsController : Controller {
    private readonly ILogger<ReportsController> _logger;
    private readonly GestiuneaStoculuiDbContext _context;

    public ReportsController(ILogger<ReportsController> logger, GestiuneaStoculuiDbContext context) {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IActionResult StockReports() {
        var stockReports = _context.ProductReports.Include(pr => pr.Product).ToList(); 
        return View(stockReports);
    }
}