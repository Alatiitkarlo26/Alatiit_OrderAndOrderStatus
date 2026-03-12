using GadgetStoreModels;

namespace GadgetStoreDataService
{
   
        public class DataRepository
        {
            public List<Product> GetProducts()
            {
                return new List<Product>
            {
            new Product { Name = "Laptop", Price = 800 },
            new Product { Name = "Smartphone", Price = 500 },
            new Product { Name = "Headphones", Price = 75 },
            new Product { Name = "Keyboard", Price = 40 },
            new Product { Name = "Mouse", Price = 25 },
            new Product { Name = "PC", Price = 2000 },
            new Product { Name = "Monitor", Price = 150 }
            };

            }
            public static List<Transaction> History = new List<Transaction>();
        } 
}