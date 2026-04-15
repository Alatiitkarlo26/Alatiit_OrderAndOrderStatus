using System;
using System.Linq;
using GadgetStoreModels;
using GadgetStoreDataService;

namespace GadgetStoreAppService
{
    public class StoreEngine
    {
     
        private readonly GadgetStoreJsonData _dataService = new GadgetStoreJsonData();
        private readonly GadgetStoreDbData _dbService = new GadgetStoreDbData();

        public decimal CalculateTotalWithTax(decimal subtotal)
        {
            // Formula 
            return subtotal * 1.12m; 
        }

        public bool CanPurchase(Guid productId, int requestedQty)
        {
            var product = _dataService.GetProducts().FirstOrDefault(p => p.ProductId == productId);
            return product != null && product.Stock >= requestedQty;
        }

        // Handles a sale across all platforms.
        public void ProcessSale(Guid productId, string productName, int qty, decimal total)
        {
         
            var newSale = new Transaction
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = qty,
                TotalPrice = total,
                TransactionDate = DateTime.Now
            };

            // 1. Update the inventory file.
            _dataService.UpdateProductStock(productId, qty);

            // 2. Add the record to the JSON history.
            _dataService.AddTransaction(newSale);

            // 3. Sync the record to the XAMPP MySQL database.
            _dbService.SaveTransactionToDb(newSale);
        }


        //  deletion in both storage systems.
        public void DeleteTransaction(Guid id)
        {
            _dataService.DeleteTransaction(id);
            _dbService.DeleteTransactionFromDb(id);
        }

        public void UpdateTransaction(Transaction t)
        {
            _dataService.UpdateTransaction(t);
            _dbService.UpdateTransactionInDb(t);
        }
    }
}