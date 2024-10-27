using DinocoStore.Web.Data;
using DinocoStore.Web.Models;
using DinocoStore.Web.Models.Enums;
using DinocoStore.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DinocoStore.Web.Controllers;

public class InvoicesController : Controller
{
    private readonly ApplicationDbContext _context;

    public InvoicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var invoices = await _context.Invoices
            .AsNoTracking()
            .ToListAsync();

        return View(new
        {
            Invoices = invoices,
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceItems)
            .ThenInclude(ii => ii.Product)
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return View(new
        {
            Invoice = invoice,
        });
    }

    public async Task<IActionResult> Add()
    {
        var products = await _context.Products
            .Where(p => p.Stock > 0 && p.Active == true)
            .AsNoTracking()
            .ToListAsync();

        return View(new
        {
            Products = products,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        [Bind(["Customer", "Total", "Items"])]
        AddInvoiceRequest request
    )
    {
        if (!ModelState.IsValid)
        {
            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Ops! Fill all the required fields";

            return RedirectToAction(nameof(Add));
        }

        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var items = request.Items
                .Select(i => new InvoiceItem
                {
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Quantity = i.Quantity
                })
                .ToList();

            foreach (var item in items)
            {
                var product = await _context.Products.SingleAsync(p => p.Id == item.ProductId);

                if (product.Stock < item.Quantity)
                    throw new Exception("Insufficient stock");

                product.Stock -= item.Quantity;
                _context.Products.Update(product);
            }

            var invoice = new Invoice
            {
                Customer = request.Customer,
                DateTime = DateTime.UtcNow,
                State = InvoiceState.Finished,
                Total = request.Total,
                InvoiceItems = items
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            TempData["MessageType"] = "success";
            TempData["MessageText"] = "Good job! Invoice created successfully";

            return RedirectToAction(nameof(Add));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Wrong job! Invoice not created. " + ex.Message;

            return RedirectToAction(nameof(Add));
        }
    }

    public async Task<IActionResult> Anulate(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceItems)
            .SingleOrDefaultAsync(i => i.Id == id);

        if (invoice is null)
        {
            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Wrong job! Invoice not found.";

            return RedirectToAction(nameof(Add));
        }

        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var item in invoice.InvoiceItems)
            {
                await _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, p => p.Stock + item.Quantity));
            }

            invoice.State = InvoiceState.Annulled;
            _context.Invoices.Update(invoice);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["MessageType"] = "success";
            TempData["MessageText"] = "Good job! Invoice annulled successfully";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            TempData["MessageType"] = "danger";
            TempData["MessageText"] = "Wrong job! Invoice not annulled. " + ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
