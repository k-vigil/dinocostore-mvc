namespace DinocoStore.Web.Models;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
