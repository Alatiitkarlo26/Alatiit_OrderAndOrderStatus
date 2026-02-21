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
                
                Console.Clear();
                Console.WriteLine("Welcome To Karlo's Gadget Store!");
                Console.WriteLine(" PRODUCT MENU ");
                Console.WriteLine("1. Laptop - $800");
                Console.WriteLine("2. Smartphone - $500");
                Console.WriteLine("3. Headphones - $75");
                Console.WriteLine("4. Keyboard - $40");
                Console.WriteLine("5. Mouse - $25");

                Console.Write("\nChoose a product (1-5): ");
                int choice = Convert.ToInt16(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        productName = "Laptop";
                        price = 800;
                        break;
                    case 2:
                        productName = "Smartphone";
                        price = 500;
                        break;
                    case 3:
                        productName = "Headphones";
                        price = 75;
                        break;
                    case 4:
                        productName = "Keyboard";
                        price = 40;
                        break;
                    case 5:
                        productName = "Mouse";
                        price = 25;
                        break;


                    default:
                        Console.WriteLine("Invalid choice.");
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

            Console.WriteLine("\n BILL SUMMARY ");
            Console.WriteLine("Subtotal: $" + finalTotal);

          
            double tax = finalTotal * 0.12;
            double totalWithTax = finalTotal + tax;

            Console.WriteLine("12% Tax: $" + tax);
            Console.WriteLine("Total With Tax: $" + totalWithTax);

           
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