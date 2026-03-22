using GadgetStoreModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GadgetStoreDataService
{
    public class GadgetStoreJsonData
    {
        private List<Product> _products = new List<Product>();
        private List<Transaction> _transactions = new List<Transaction>();

        private readonly string _productFileName;
        private readonly string _transactionFileName;

        public GadgetStoreJsonData()
        {
            
            _productFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Products.json");
            _transactionFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Transactions.json");

            InitializeData();
        }

        private void InitializeData()
        {
            
            if (File.Exists(_productFileName))
            {
                RetrieveProducts();
            }
            else
            {
                
                _products = new List<Product>
                {
                    new Product { ProductId = Guid.NewGuid(), Name = "Laptop", Price = 800, Stock = 10 },
                    new Product { ProductId = Guid.NewGuid(), Name = "Smartphone", Price = 500, Stock = 15 },
                    new Product { ProductId = Guid.NewGuid(), Name = "Headphones", Price = 75, Stock = 30 },
                    new Product { ProductId = Guid.NewGuid(), Name = "Keyboard", Price = 40, Stock = 20 },
                    new Product { ProductId = Guid.NewGuid(), Name = "Mouse", Price = 25, Stock = 25 },
                    new Product { ProductId = Guid.NewGuid(), Name = "PC", Price = 2000, Stock = 5 },
                    new Product { ProductId = Guid.NewGuid(), Name = "Monitor", Price = 150, Stock = 12 }
                };
                SaveProducts();
            }

            
            if (File.Exists(_transactionFileName))
            {
                RetrieveTransactions();
            }
        }

        #region Product Logic
        public List<Product> GetProducts()
        {
            RetrieveProducts();
            return _products;
        }

        public void AddProduct(Product product)
        {
            if (product.ProductId == Guid.Empty) product.ProductId = Guid.NewGuid();
            _products.Add(product);
            SaveProducts();
        }

        private void SaveProducts()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_products, options);
            File.WriteAllText(_productFileName, jsonString);
        }

        private void RetrieveProducts()
        {
            if (!File.Exists(_productFileName)) return;
            string jsonString = File.ReadAllText(_productFileName);
            _products = JsonSerializer.Deserialize<List<Product>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Product>();
        }
        #endregion

        #region Transaction Logic
        public List<Transaction> GetHistory()
        {
            RetrieveTransactions();
            return _transactions;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.TransactionId == Guid.Empty) transaction.TransactionId = Guid.NewGuid();
            transaction.TransactionDate = DateTime.Now;

            _transactions.Add(transaction);
            SaveTransactions();
        }

        private void SaveTransactions()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_transactions, options);
            File.WriteAllText(_transactionFileName, jsonString);
        }

        private void RetrieveTransactions()
        {
            if (!File.Exists(_transactionFileName)) return;
            string jsonString = File.ReadAllText(_transactionFileName);
            _transactions = JsonSerializer.Deserialize<List<Transaction>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Transaction>();
        }
        #endregion
    }
}