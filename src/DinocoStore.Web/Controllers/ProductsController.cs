using DinocoStore.Web.Data;
using DinocoStore.Web.Models;
using DinocoStore.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DinocoStore.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .AsNoTracking()
            .ToListAsync();

        return View(new
        {
            Products = products,
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

        return View(new
        {
            Product = product,
        });
    }

    public IActionResult Add() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        [Bind(["Description", "Stock", "Price"])]
        AddProductRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Ops! Fill all the required fields";

            return RedirectToAction(nameof(Add));
        }

        var product = new Product
        {
            Description = request.Description,
            Stock = request.Stock,
            Price = request.Price,
            Active = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        TempData["MessageType"] = "success";
        TempData["MessageText"] = "Good job! Product added successfully";

        return RedirectToAction(nameof(Add));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

        return View(new
        {
            Product = product,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind(["Id", "Description", "Stock", "Price", "Active"])]
        UpdateProductRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Ops! Fill all the required fields";

            return RedirectToAction(nameof(Edit), new { id });
        }

        var product = await _context.Products.SingleAsync(p => p.Id == id);

        product.Description = request.Description;
        product.Stock = request.Stock;
        product.Price = request.Price;
        product.Active = request.Active;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        TempData["MessageType"] = "success";
        TempData["MessageText"] = "Good job! Product updated successfully";

        return RedirectToAction(nameof(Index));
    }
}
