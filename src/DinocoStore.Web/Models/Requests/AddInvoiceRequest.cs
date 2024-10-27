namespace DinocoStore.Web.Models.Requests;

public record AddInvoiceRequest(
    string Customer,
    decimal Total,
    List<AddInvoiceItemRequest> Items
);

public record AddInvoiceItemRequest(
    int ProductId,
    int Quantity,
    decimal Price
);
