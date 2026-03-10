using System;
using System.Collections.Generic;
using System.Linq;
using GadgetStoreModels;
using GadgetStoreAppService;
using GadgetStoreDataService;

namespace GadgetStore
{
    class Program
    {

        private static DataRepository _data = new DataRepository();
        private static StoreEngine _engine = new StoreEngine();

        static void Main(string[] args)
        {
            bool exitApp = false;

            while (!exitApp)
            {
                DisplayMainMenu();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RunShoppingSession();
                        break;
                    case "2":
                        ViewHistory();
                        break;
                    case "3":
                        exitApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("==== KARLO'S GADGET STORE ====");
            Console.WriteLine("1. New Transaction");
            Console.WriteLine("2. View Sales History");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect Action: ");
        }

        static void RunShoppingSession()
        {
            double subtotal = 0;
            List<string> purchasedItems = new List<string>();
            string continueShopping = "y";

            var inventory = _data.GetProducts();

            while (continueShopping == "y")
            {
                Console.Clear();
                Console.WriteLine("--- AVAILABLE PRODUCTS ---");
                foreach (var p in inventory)
                {
                    Console.WriteLine($"{p.Name,-15} | ${p.Price:N2}");
                }

                Console.Write("\nType product name: ");
                string choice = Console.ReadLine()?.ToLower();

                var selected = inventory.FirstOrDefault(p => p.Name.ToLower() == choice);

                if (selected != null)
                {
                    Console.Write($"Quantity of {selected.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int qty))
                    {
                        subtotal += (selected.Price * qty);
                        purchasedItems.Add($"{qty}x {selected.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found!");
                }

                Console.Write("\nAdd more? (y/n): ");
                continueShopping = Console.ReadLine()?.ToLower();
            }

            if (subtotal > 0)
            {
                ProcessCheckout(subtotal, string.Join(", ", purchasedItems));
            }
        }

        static void ProcessCheckout(double subtotal, string summary)
        {
        
            double totalWithTax = _engine.CalculateTotalWithTax(subtotal);

            Console.WriteLine("\n--- CHECKOUT ---");
            Console.WriteLine($"Subtotal:  ${subtotal:N2}");
            Console.WriteLine($"Total (Inc. 12% Tax): ${totalWithTax:N2}");

            Console.Write("\nEnter cash amount: $");
            double cash = Convert.ToDouble(Console.ReadLine());

            if (cash >= totalWithTax)
            {
                Console.WriteLine($"Change: ${(cash - totalWithTax):N2}");

             
                _engine.SaveTransaction(summary, totalWithTax);

                Console.WriteLine("Transaction Complete!");
            }
            else
            {
                Console.WriteLine("Insufficient funds. Transaction cancelled.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void ViewHistory()
        {
            Console.Clear();
            Console.WriteLine("=== TRANSACTION HISTORY ===");

          
            var history = DataRepository.History;

            if (history.Count == 0)
            {
                Console.WriteLine("No records found.");
            }
            else
            {
                foreach (var t in history)
                {
                    Console.WriteLine($"[{t.Date:HH:mm}] {t.Summary} | Paid: ${t.TotalPaid:N2}");
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}