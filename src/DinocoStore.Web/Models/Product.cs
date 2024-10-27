using System.ComponentModel.DataAnnotations;

namespace DinocoStore.Web.Models;

public class Product
{
    public int Id { get; init; }
    [MaxLength(50)]
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public bool Active { get; set; }
    public ICollection<InvoiceItem> InvoiceItems { get; init; } = null!;
}
