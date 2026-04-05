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
        private static GadgetStoreJsonData _dataService = new GadgetStoreJsonData();
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
                    case "1": RunShoppingSession(); break;
                    case "2": ViewHistory(); break;
                    case "3": exitApp = true; break;
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
            List<(Product Item, int Qty)> cart = new List<(Product Item, int Qty)>();
            var inventory = _dataService.GetProducts();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- AVAILABLE PRODUCTS ---");
                foreach (var p in inventory)
                {
                 
                    Console.WriteLine($"{p.Name,-15} | ${p.Price,8:N2} | Stock: {p.Stock}");
                }

                Console.Write("\nEnter Product Name (or 'done' to checkout): ");
                string choice = Console.ReadLine()?.ToLower();
                if (choice == "done") break;

                var selected = inventory.FirstOrDefault(p => p.Name.ToLower() == choice);
                if (selected != null)
                {
                    Console.Write($"Quantity of {selected.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                    {
                        if (qty <= selected.Stock)
                        {
                            cart.Add((selected, qty));
                            Console.WriteLine("Added to cart!");
                        }
                        else
                        {
                            Console.WriteLine("Error: Not enough stock available!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Product not found!");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            if (cart.Count > 0) ProcessCheckout(cart);
        }

        static void ProcessCheckout(List<(Product Item, int Qty)> cart)
        {
            decimal subtotal = cart.Sum(c => c.Item.Price * c.Qty);
            decimal totalWithTax = _engine.CalculateTotalWithTax(subtotal);

            Console.Clear();
            Console.WriteLine("--- CHECKOUT ---");
            foreach (var entry in cart)
            {
                Console.WriteLine($"{entry.Qty}x {entry.Item.Name,-15} | ${(entry.Item.Price * entry.Qty):N2}");
            }
            Console.WriteLine(new string('-', 30));
            Console.WriteLine($"Total (Inc. 12% Tax): ${totalWithTax:N2}");

            Console.Write("\nEnter Cash Amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal cash))
            {
                if (cash >= totalWithTax)
                {
                    Console.WriteLine($"Change: ${(cash - totalWithTax):N2}");

                    foreach (var entry in cart)
                    {
                        _engine.ProcessSale(
                            entry.Item.ProductId,
                            entry.Item.Name,
                            entry.Qty,
                            (entry.Item.Price * entry.Qty)
                        );
                    }
                    Console.WriteLine("\nTransaction saved successfully!");
                }
                else
                {
                    Console.WriteLine("Insufficient funds. Transaction cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid cash input.");
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        static void ViewHistory()
        {
            Console.Clear();
            Console.WriteLine("=== TRANSACTION HISTORY ===");

            var history = _dataService.GetHistory();

            if (history == null || history.Count == 0)
            {
                Console.WriteLine("No records found.");
                Console.ReadKey();
                return;
            }

            foreach (var t in history)
            {
                Console.WriteLine($"ID: {t.TransactionId}");
                Console.WriteLine($"[{t.TransactionDate:MM/dd HH:mm}] {t.ProductName} ({t.Quantity} units) | Total: ${t.TotalPrice:N2}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\n[E] Edit | [D] Delete | [Any other key] Back");
            Console.Write("Action: ");
            string action = Console.ReadLine()?.ToUpper();

            if (action == "D" || action == "E")
            {
                Console.Write("Enter ID (First 8 chars): ");
                string inputId = Console.ReadLine();
                var target = history.FirstOrDefault(t => t.TransactionId.ToString().StartsWith(inputId));

                if (target != null)
                {
                    if (action == "D")
                    {
                        _engine.DeleteTransaction(target.TransactionId);
                        Console.WriteLine("Record Deleted.");
                    }
                    else if (action == "E")
                    {
                        Console.Write($"New Name (Current: {target.ProductName}): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName)) target.ProductName = newName;

                        Console.Write($"New Total (Current: {target.TotalPrice}): ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal newPrice)) target.TotalPrice = newPrice;

                        _engine.UpdateTransaction(target);
                        Console.WriteLine("Record Updated.");
                    }
                }
                else Console.WriteLine("Transaction ID not found.");

                Console.ReadKey();
            }
        }
    }
}