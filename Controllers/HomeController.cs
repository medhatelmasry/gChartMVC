using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gChartMVC.Models;
using gChartMVC.NW;
using Microsoft.EntityFrameworkCore;

namespace gChartMVC.Controllers;

public class HomeController : Controller
{
    private readonly NorthwindContext _northwindContext;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, NorthwindContext northwindContext)
    {
        _logger = logger;
        _northwindContext = northwindContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<JsonResult> ChartData()
    {
        var query = await _northwindContext.Products
              .Include(c => c.Category)
              .GroupBy(p => p.Category!.CategoryName)
              .Select(g => new
              {
                  Name = g.Key,
                  Count = g.Count()
              })
              .OrderByDescending(cp => cp.Count)
              .ToListAsync();

        return Json(query);
    }
}