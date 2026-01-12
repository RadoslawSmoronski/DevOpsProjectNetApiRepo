var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var products = new List<Product>
{
    new(1, "Laptop", 999.99m),
    new(2, "Phone", 599.99m)
};

app.MapGet("/products", () => Results.Ok(products));

app.MapGet("/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapGet("/products/search", (string? name) =>
{
    if (string.IsNullOrEmpty(name)) return Results.BadRequest("Name is required");
    var found = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    return Results.Ok(found);
});

app.MapPost("/products", (Product product) =>
{
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapDelete("/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound();
    
    products.Remove(product);
    return Results.NoContent();
});

app.Run();

record Product(int Id, string Name, decimal Price);
