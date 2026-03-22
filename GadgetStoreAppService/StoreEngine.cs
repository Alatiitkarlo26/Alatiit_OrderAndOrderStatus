using GadgetStoreModels;
using GadgetStoreDataService;
using System;

namespace GadgetStoreAppService
{
    public class StoreEngine
    {
  
        private readonly GadgetStoreJsonData _dataService = new GadgetStoreJsonData();
        private readonly GadgetStoreDbData _dbService = new GadgetStoreDbData();

        public decimal CalculateTotalWithTax(decimal subtotal)
        {
            return subtotal + (subtotal * 0.12m);
        }

        public void SaveTransaction(Guid productId, string productName, int qty, decimal total)
        {
         
            var transactionRecord = new Transaction
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = qty,
                TotalPrice = total,
                TransactionDate = DateTime.Now
            };

           
            _dataService.AddTransaction(transactionRecord);

         
            _dbService.SaveTransactionToDb(transactionRecord);
        }
    }
}