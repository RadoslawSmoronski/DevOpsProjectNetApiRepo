using Services;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var productService = new ProductService();

app.MapGet("/products", () => Results.Ok(productService.Get()));

app.MapGet("/products/{id:int}", (int id) =>
{
    var product = productService.Get(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapGet("/products/search", (string? name) =>
{
    if (string.IsNullOrEmpty(name)) return Results.BadRequest("Name is required");
    return Results.Ok(productService.Get(name));
});

app.MapPost("/products", (ProductRequest productRequest) =>
{
    try
    {
        var product = productService.Create(productRequest.Name, productRequest.Price);
        return Results.Created($"/products/{product.Id}", product);
    }
    catch(ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch(Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }
});

app.MapDelete("/products/{id:int}", (int id) =>
{
    try
    {
        productService.Delete(id);
        return Results.NoContent();
    }
    catch(KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch(Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }

});

app.Run();
