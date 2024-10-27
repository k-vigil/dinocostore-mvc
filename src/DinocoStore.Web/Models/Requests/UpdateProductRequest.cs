namespace DinocoStore.Web.Models.Requests;

public record UpdateProductRequest(
    int Id,
    string Description,
    int Stock,
    decimal Price,
    bool Active
);
