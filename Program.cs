using GadgetStoreAppService;
using GadgetStoreDataService;
using GadgetStoreModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;

namespace GadgetStore
{
    class Program
    {
        // We initialize the data service (for reading JSON) 
        // and the engine (for processing logic like sales and updates).
        private static GadgetStoreJsonData _dataService = new GadgetStoreJsonData();
        private static StoreEngine _engine = new StoreEngine();

        static void Main(string[] args)
        {
            bool exitApp = false;
            // Keeps the program running until "Exit" is chosen.
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

        // SHOPPING LOGIC: Handles picking items and checking stock.
        static void RunShoppingSession()
        {
            // CART: A temporary list of "Tuples". It stores the Product object 
            // and the specific quantity the user wants to buy.
            List<(Product Item, int Qty)> cart = new List<(Product Item, int Qty)>();

            // Gets the latest stock levels from the JSON file
            var inventory = _dataService.GetProducts();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- AVAILABLE PRODUCTS ---");
                foreach (var p in inventory)
                {
                    // :N2 format ensures price shows as $0.00, and -15 pads the name for alignment.
                    Console.WriteLine($"{p.Name,-15} | ${p.Price,8:N2} | Stock: {p.Stock}");
                }

                Console.Write("\nEnter Product Name (or 'done' to checkout): ");
                string choice = Console.ReadLine()?.ToLower();
                if (choice == "done") break;

                // SEARCH: Finds the product in the list that matches the user's typed name.
                var selected = inventory.FirstOrDefault(p => p.Name.ToLower() == choice);

                if (selected != null)
                {
                    Console.Write($"Quantity of {selected.Name}: ");
                    if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                    { 
                        // Ensures we don't sell more than we actually have in JSON.
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

            // Only proceed to checkout if the user actually added something to the cart.
            if (cart.Count > 0) ProcessCheckout(cart);
        }

        // FINANCIAL LOGIC: Calculates totals, taxes, and handles the cash payment.
        static void ProcessCheckout(List<(Product Item, int Qty)> cart)
        {   
            decimal subtotal = cart.Sum(c => c.Item.Price * c.Qty); // MATH: Sums up (Price * Quantity) for every item in the cart.
            decimal totalWithTax = _engine.CalculateTotalWithTax(subtotal); // TAX: Calls the Engine to apply the 12% tax.

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
                if (cash >= totalWithTax) // PAYMENT CHECK: Only save if the user provided enough money.
                {
                    Console.WriteLine($"Change: ${(cash - totalWithTax):N2}");

                    // SAVE: For every item in the cart, tell the Engine to:
                    // 1. Update JSON stock. 2. Save JSON transaction. 3. Save MySQL transaction.
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

        // MANAGEMENT LOGIC: View sales, and provides options to Edit or Delete records.
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

            foreach (var t in history) // Lists every transaction found in the JSON/Database.
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
                // Since GUIDs are long, we let the user type just the first few characters.
                Console.Write("Enter ID (First 8 chars): ");
                string inputId = Console.ReadLine();
                var target = history.FirstOrDefault(t => t.TransactionId.ToString().StartsWith(inputId));

                if (target != null)
                {  
                    // DELETE: Tells the engine to remove it from both JSON and MySQL.
                    if (action == "D")
                    {
                        _engine.DeleteTransaction(target.TransactionId);
                        Console.WriteLine("Record Deleted.");
                    }
                    // EDIT: Allows updating the name or quantity of a past sale.
                    else if (action == "E")
                    {
                        
                        Console.Write($"New Name (Current: {target.ProductName}): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName)) target.ProductName = newName;

                      
                        Console.Write($"New Quantity (Current: {target.Quantity}): ");
                        if (int.TryParse(Console.ReadLine(), out int newQty) && newQty > 0)
                        {
                            // AUTO-RECALCULATION: 
                            // 1. Find what the price was per unit (Total / Qty).
                            // 2. Multiply that price by the NEW quantity.
                            decimal unitPrice = target.TotalPrice / target.Quantity;
                            target.Quantity = newQty;
                            target.TotalPrice = unitPrice * newQty;

                            Console.WriteLine($"Quantity updated! New Total: ${target.TotalPrice:N2}");
                        }

                        // SYNC: Sends the modified object to the engine to update both storage systems.
                        _engine.UpdateTransaction(target);
                        Console.WriteLine("Record Updated successfully!");
                    }
                }
                else Console.WriteLine("Transaction ID not found.");

                Console.ReadKey();
            }
        }
    }
}