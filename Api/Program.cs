using Services;
using Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

var productService = new ProductService();

app.MapGet("/products", () => 
{
    Log.Information("Fetching all products");
    var products = productService.Get();
    Log.Information("Returned {Count} products", products.Count());
    return Results.Ok(products);
});

app.MapGet("/products/{id:int}", (int id) =>
{
    Log.Information("Fetching product with ID: {ProductId}", id);
    var product = productService.Get(id);
    if (product is not null)
    {
        Log.Information("Found product: {ProductName}", product.Name);
        return Results.Ok(product);
    }
    Log.Warning("Product with ID: {ProductId} not found", id);
    return Results.NotFound();
});

app.MapGet("/products/search", (string? name) =>
{
    Log.Information("Searching products by name: {SearchName}", name);
    if (string.IsNullOrEmpty(name))
    {
        Log.Warning("Search attempted without providing name");
        return Results.BadRequest("Name is required");
    }
    var products = productService.Get(name);
    Log.Information("Found {Count} products for query: {SearchName}", products.Count(), name);
    return Results.Ok(products);
});

app.MapPost("/products", (ProductRequest productRequest) =>
{
    Log.Information("Creating new product: {ProductName}, Price: {Price}", productRequest.Name, productRequest.Price);
    try
    {
        var product = productService.Create(productRequest.Name, productRequest.Price);
        Log.Information("Product created with ID: {ProductId}", product.Id);
        return Results.Created($"/products/{product.Id}", product);
    }
    catch(ArgumentException ex)
    {
        Log.Warning("Validation error while creating product: {ErrorMessage}", ex.Message);
        return Results.BadRequest(ex.Message);
    }
    catch(Exception ex)
    {
        Log.Error(ex, "Error creating product: {ErrorMessage}", ex.Message);
        return Results.InternalServerError(ex.Message);
    }
});

app.MapDelete("/products/{id:int}", (int id) =>
{
    Log.Information("Deleting product with ID: {ProductId}", id);
    try
    {
        productService.Delete(id);
        Log.Information("Successfully deleted product with ID: {ProductId}", id);
        return Results.NoContent();
    }
    catch(KeyNotFoundException)
    {
        Log.Warning("Attempted to delete non-existent product with ID: {ProductId}", id);
        return Results.NotFound();
    }
    catch(Exception ex)
    {
        Log.Error(ex, "Error deleting product with ID: {ProductId}", id);
        return Results.InternalServerError(ex.Message);
    }

});

app.Run();
