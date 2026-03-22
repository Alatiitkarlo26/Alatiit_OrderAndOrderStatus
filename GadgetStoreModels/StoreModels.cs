namespace GadgetStoreModels
{
    public class Product
    {
      
        public Guid ProductId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

       
        public int Stock { get; set; }
    }

    public class Transaction
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();

      
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}

