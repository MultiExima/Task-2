using System;
using System.Globalization;
using ShoppingListProject.Services;
using ShoppingListProject.Models;

namespace ShoppingListProject
{
    public class Program
    {
        private static ShoppingList _shoppingList;
        private static FileStorage _fileStorage;
        private const string FILENAME = "shoppinglist.csv";

        public static void Main(string[] args)
        {
            _fileStorage = new FileStorage(FILENAME);
            _shoppingList = _fileStorage.Load();

            Console.WriteLine("Shopping-list");
            Console.WriteLine($"Data loaded from {FILENAME} ({_shoppingList.GetAllPurchases().Count} items).");

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleAddPurchase();
                        break;
                    case "2":
                        HandleRemovePurchase();
                        break;
                    case "3":
                        HandlePrintRange();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Error: The number must be in the range [1;4].");
                        break;
                }
            }

            _fileStorage.Save(_shoppingList);
            Console.WriteLine($"Data saved to {FILENAME}. Exiting program.");
        }
        private static void PrintMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1 Add a purchase");
            Console.WriteLine("2 Delete a purchase by number");
            Console.WriteLine("3 Show purchases for the period");
            Console.WriteLine("4 Save and Exit");
            Console.Write("> ");
        }

        private static void HandleAddPurchase()
        {
            try
            {
                Console.WriteLine("\nNew purchase");

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Commentary: ");
                string comment = Console.ReadLine();

                Console.Write("Amount (dollars): ");

                decimal credits;

                while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out credits) || credits < 0)
                {
                    Console.Write("Error: Invalid amount. Please enter a positive number (e.g., 10.50): ");
                }

                Console.Write("Date (d.M.yyyy): ");

                PurchaseDate date;
                while (true)
                {
                    try
                    {
                        date = PurchaseDate.Parse(Console.ReadLine());
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.Write("Error: Invalid date format! Please use d.M.yyyy (e.g., 25.12.2023): ");
                    }
                }

                Purchase purchase = new Purchase
                {
                    Name = string.IsNullOrWhiteSpace(name) ? "(No Name)" : name,
                    Comment = comment,
                    Credits = credits,
                    Date = date
                };

                _shoppingList.AddPurchase(purchase);
                Console.WriteLine("Purchase added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding purchase: {ex.Message}");
            }
        }

        private static void HandleRemovePurchase()
        {
            Console.WriteLine("\n--- Select purchase to delete ---");
            _shoppingList.PrintAllPurchases();

            if (_shoppingList.GetAllPurchases().Count == 0)
            {
                return;
            }

            Console.Write("\nEnter the purchase number to be deleted: ");

            int number;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Error: Invalid number. Please enter a number: ");
            }

            if (_shoppingList.RemovePurchase(number))
            {
                Console.WriteLine("Purchase deleted.");
            }
            else
            {
                Console.WriteLine("Error: Wrong purchase number!");
            }
        }


        private static void HandlePrintRange()
        {
            try
            {
                Console.Write("\nStart date (d.M.yyyy): ");
                PurchaseDate startDate;
                while (true)
                {
                    try
                    {
                        startDate = PurchaseDate.Parse(Console.ReadLine());
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.Write("Error: Invalid date format! Please use d.M.yyyy: ");
                    }
                }

                Console.Write("End date (d.M.yyyy): ");
                PurchaseDate endDate;
                while (true)
                {
                    try
                    {
                        endDate = PurchaseDate.Parse(Console.ReadLine());
                        if (endDate < startDate)
                        {
                            Console.Write("Error: End date must be after or equal to start date. Try again: ");
                            continue;
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.Write("Error: Invalid date format! Please use d.M.yyyy: ");
                    }
                }

                var purchases = _shoppingList.GetPurchasesInRange(startDate, endDate);

                Console.WriteLine($"\nPurchases in the range from {startDate} to {endDate}:");

                if (purchases.Count == 0)
                {
                    Console.WriteLine("No purchases in the specified range were found. :(");
                    return;
                }

                for (int i = 0; i < purchases.Count; i++)
                {
                    Console.WriteLine("\n---");
                    Console.WriteLine(purchases[i]);
                    Console.WriteLine("---");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}