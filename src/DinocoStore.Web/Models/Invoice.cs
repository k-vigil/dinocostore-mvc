using DinocoStore.Web.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DinocoStore.Web.Models;

public class Invoice
{
    public int Id { get; init; }
    [MaxLength(50)]
    public string Customer { get; init; } = string.Empty;
    public InvoiceState State { get; set; }
    public DateTime DateTime { get; init; }
    public decimal Total { get; init; }
    public ICollection<InvoiceItem> InvoiceItems { get; init; } = null!;
}
