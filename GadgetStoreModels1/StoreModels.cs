namespace GadgetStoreModels
{
    // Represents an item for sale in your inventory.
    public class Product
    {
        // Unique ID for each product. Essential for searching and updating specific items.
        public Guid ProductId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        // Decimal is used for money to prevent rounding errors that happen with 'double' or 'float'.
        public decimal Price { get; set; }

        public int Stock { get; set; }
    }

    // Represents a completed sale record.
    public class Transaction
    {
        // Unique ID for the receipt. Used to find specific records in Edit/Delete functions.
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        // Links this sale back to the original Product ID.
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        // How many units were bought in this specific transaction.
        public int Quantity { get; set; }

        // Total price after math (Price * Qty + Tax).
        public decimal TotalPrice { get; set; }

        // Timestamp to track when the sale happened.
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}