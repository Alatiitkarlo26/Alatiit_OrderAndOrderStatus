using GadgetStoreModels;
using GadgetStoreDataService;

namespace GadgetStoreAppService
{
    public class StoreEngine
    {
        public double CalculateTotalWithTax(double subtotal)
        {
            return subtotal + (subtotal * 0.12);
        }

        public void SaveTransaction(string items, double total)
        {
            DataRepository.History.Add(new Transaction
            {
                Date = DateTime.Now,
                Summary = items,
                TotalPaid = total
            });
        }








    }
}