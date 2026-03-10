namespace GadgetStoreModels
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Summary { get; set; }
        public double TotalPaid { get; set; }
    }
}

