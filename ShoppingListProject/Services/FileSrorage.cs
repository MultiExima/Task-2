using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ShoppingListProject.Models;
namespace ShoppingListProject.Services
{
    public class FileStorage
    {
        private readonly string _filePath;

        private const char SEPARATOR = ';';

        public FileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(ShoppingList list)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine("Name;Comment;Credits;Date");

                    foreach (var purchase in list.GetAllPurchases())
                    {
                        string name = Escape(purchase.Name);
                        string comment = Escape(purchase.Comment);
                        string credits = purchase.Credits.ToString(CultureInfo.InvariantCulture);
                        string date = purchase.Date.ToString();

                        writer.WriteLine($"{name}{SEPARATOR}{comment}{SEPARATOR}{credits}{SEPARATOR}{date}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR] Could not save data to {_filePath}. Error: {ex.Message}");
            }
        }

        public ShoppingList Load()
        {
            ShoppingList list = new ShoppingList();
            if (!File.Exists(_filePath))
            {
                return list;
            }

            try
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    if (reader.ReadLine() == null)
                    {
                        return list;
                    }

                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            string[] parts = SplitCsvLine(line);

                            if (parts.Length != 4)
                            {
                                Console.WriteLine($"[Load Warning] Skipping malformed line: {line}");
                                continue;
                            }

                            Purchase purchase = new Purchase
                            {
                                Name = Unescape(parts[0]),
                                Comment = Unescape(parts[1]),
                                Credits = decimal.Parse(parts[2], CultureInfo.InvariantCulture),
                                Date = PurchaseDate.Parse(parts[3])
                            };
                            list.AddPurchase(purchase);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Load Error] Error parsing line: {line}. Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR] Could not load data from {_filePath}. Error: {ex.Message}");
            }
            return list;
        }

        private string Escape(string value)
        {
            if (value.Contains(SEPARATOR) || value.Contains('\"'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        private string Unescape(string value)
        {
            if (value.StartsWith('\"') && value.EndsWith('\"'))
            {
                value = value.Substring(1, value.Length - 2);
                return value.Replace("\"\"", "\"");
            }
            return value;
        }

        private string[] SplitCsvLine(string line)
        {
            List<string> parts = new List<string>();
            StringBuilder currentPart = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '\"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '\"')
                    {
                        currentPart.Append('\"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == SEPARATOR && !inQuotes)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
                else
                {
                    currentPart.Append(c);
                }
            }
            parts.Add(currentPart.ToString());
            return parts.ToArray();
        }
    }
}