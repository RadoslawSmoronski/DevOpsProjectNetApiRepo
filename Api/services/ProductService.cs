using Models;

namespace Services
{
    internal class ProductService
    {
        private List<Product> products = new List<Product>
        {
            new(1, "Laptop", 999.99m),
            new(2, "Phone", 599.99m)
        };

        public Product Create(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3) 
                throw new ArgumentException("Name must be at least 3 characters long", nameof(name));
            if (price < 0) 
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative");
            
            var id = products.Max(x => x.Id) + 1;
            var product = new Product(id, name, price);
            products.Add(product);
            return product;
        }

        public List<Product> Get()
        {
           return products;
        }

        public Product? Get(int id)
        {
           return products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> Get(string name)
        {
           return products.Where(p => p.Name == name).ToList();
        }

        public void Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product is null) throw new KeyNotFoundException();
            
            products.Remove(product);
        }
    }
}