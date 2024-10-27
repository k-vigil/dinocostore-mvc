using DinocoStore.Web.Models;
using DinocoStore.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DinocoStore.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; init; }
    public DbSet<Invoice> Invoices { get; init; }
    public DbSet<InvoiceItem> InvoiceItems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Invoice>()
            .Property(e => e.State)
            .HasConversion(new EnumToStringConverter<InvoiceState>());
    }
}
