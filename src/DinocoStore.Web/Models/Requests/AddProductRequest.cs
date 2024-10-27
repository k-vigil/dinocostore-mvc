namespace DinocoStore.Web.Models.Requests;

public record AddProductRequest(
    string Description,
    int Stock,
    decimal Price
);
