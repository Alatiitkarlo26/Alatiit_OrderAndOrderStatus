namespace ALATIIIT
{
    internal class Program
    {
        static void Main(string[] args)
        {

            double finalTotal = 0;
            string continueShopping = "y";

            while (continueShopping == "y")
            {
                string productName = "";
                double price = 0;
                int quantity;

                Console.WriteLine("\n--------------- PRODUCT MENU ---------------");
                Console.WriteLine("Laptop        - $800");
                Console.WriteLine("Smartphone    - $500");
                Console.WriteLine("Headphones    - $75");
                Console.WriteLine("Keyboard      - $40");
                Console.WriteLine("Mouse         - $25");
                Console.WriteLine("PC            - $2000");
                Console.WriteLine("MousePad      - $15");
                Console.WriteLine("GamingChair   - $1000");
                Console.WriteLine("GPUBracket    - $5");
                Console.WriteLine("Monitor       - $150");
                Console.WriteLine("--------------------------------------------");
                Console.Write("\nChoose a product (Input the name of the product): ");
                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "laptop": productName = "Laptop"; price = 800; break;
                    case "smartphone": productName = "Smartphone"; price = 500; break;
                    case "headphones": productName = "Headphones"; price = 75; break;
                    case "keyboard": productName = "Keyboard"; price = 40; break;
                    case "mouse": productName = "Mouse"; price = 25; break;
                    case "pc": productName = "PC"; price = 2000; break;
                    case "mousepad": productName = "MousePad"; price = 15; break;
                    case "gamingchair": productName = "GamingChair"; price = 1000; break;
                    case "gpubracket": productName = "GPUBracket"; price = 5; break;
                    case "monitor": productName = "Monitor"; price = 150; break;
                    default:
                        Console.WriteLine("Invalid product name.");
                        continue;
                }


                Console.Write("Enter quantity: ");
                quantity = Convert.ToInt16(Console.ReadLine());

                double total = price * quantity;
                finalTotal += total;

                Console.WriteLine("\nItem Total: $" + total);

                Console.Write("\nDo you want to buy another item? (y/n): ");
                continueShopping = Console.ReadLine().ToLower();
            }
            Console.WriteLine("\n======================================");
            Console.WriteLine("BILL SUMMARY");
            Console.WriteLine("======================================");
            Console.WriteLine("Subtotal: $" + finalTotal);


            double tax = finalTotal * 0.12;
            double totalWithTax = finalTotal + tax;

            Console.WriteLine("12% Tax: $" + tax);
            Console.WriteLine("Total With Tax: $" + totalWithTax);
            Console.WriteLine("======================================");

            Console.Write("\nEnter the money you have: $");
            double money = Convert.ToDouble(Console.ReadLine());

            if (money >= totalWithTax)
            {
                double change = money - totalWithTax;
                Console.WriteLine("Payment successful!");
                Console.WriteLine("Your change: $" + change);
            }
            else
            {
                double shortage = totalWithTax - money;
                Console.WriteLine("Not enough money!");
                Console.WriteLine("You need $" + shortage + " more.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}