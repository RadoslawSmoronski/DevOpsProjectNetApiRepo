using Models;
using Serilog;

namespace Services
{
    public class ProductService
    {
        private List<Product> products = new List<Product>
        {
            new(1, "Laptop", 999.99m),
            new(2, "Phone", 599.99m)
        };

        public Product Create(string name, decimal price)
        {
            Log.Debug("Creating product with name: {Name}, price: {Price}", name, price);
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                Log.Warning("Product creation failed: name validation error");
                throw new ArgumentException("Name must be at least 3 characters long", nameof(name));
            }
            if (price < 0)
            {
                Log.Warning("Product creation failed: price cannot be negative");
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative");
            }
            
            var id = products.Max(x => x.Id) + 1;
            var product = new Product(id, name, price);
            products.Add(product);
            Log.Debug("Product created successfully with ID: {ProductId}", id);
            return product;
        }

        public List<Product> Get()
        {
           Log.Debug("Retrieving all products from service");
           return products;
        }

        public Product? Get(int id)
        {
           Log.Debug("Retrieving product with ID: {ProductId}", id);
           var product = products.FirstOrDefault(p => p.Id == id);
           if (product is null)
               Log.Debug("Product with ID: {ProductId} not found in service", id);
           return product;
        }

        public List<Product> Get(string name)
        {
           Log.Debug("Searching products by name: {Name}", name);
           var result = products.Where(p => p.Name == name).ToList();
           Log.Debug("Found {Count} products with name: {Name}", result.Count, name);
           return result;
        }

        public void Delete(int id)
        {
            Log.Debug("Attempting to delete product with ID: {ProductId}", id);
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                Log.Warning("Cannot delete: product with ID: {ProductId} not found", id);
                throw new KeyNotFoundException();
            }
            
            products.Remove(product);
            Log.Debug("Product with ID: {ProductId} deleted successfully", id);
        }
    }
}