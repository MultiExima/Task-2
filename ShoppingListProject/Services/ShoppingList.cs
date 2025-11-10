using System;
using System.Collections.Generic;
using System.Linq;

using ShoppingListProject.Models;
namespace ShoppingListProject.Services
{
    public class ShoppingList
    {
        private List<Purchase> _items;

        public ShoppingList()
        {
            _items = new List<Purchase>();
        }

        public void AddPurchase(Purchase purchase)
        {
            _items.Add(purchase);
        }
        public bool RemovePurchase(int number)
        {
            int index = number - 1;
            if (index < 0 || index >= _items.Count)
            {
                return false;
            }
            _items.RemoveAt(index);
            return true;
        }

        public List<Purchase> GetPurchasesInRange(PurchaseDate startDate, PurchaseDate endDate)
        {
            var results = _items.Where(p => p.Date >= startDate && p.Date <= endDate)
                                .ToList();
            return results;
        }

        public void PrintAllPurchases()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("Purchase list is empty.");
                return;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                Console.WriteLine($"\n--- #{i + 1} ---");
                Console.WriteLine(_items[i]);
                Console.WriteLine("---");
            }
        }


        public List<Purchase> GetAllPurchases()
        {
            return _items;
        }
    }
}