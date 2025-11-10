using System;
using System.Globalization;
namespace ShoppingListProject.Models
{
    public struct PurchaseDate : IComparable<PurchaseDate>
    {
        public DateOnly Date { get; set; }

        public PurchaseDate(DateOnly date)
        {
            Date = date;
        }

        public static PurchaseDate Parse(string dateString)
        {
            return new PurchaseDate(DateOnly.ParseExact(dateString, "d.M.yyyy", CultureInfo.InvariantCulture));
        }

        public override string ToString()
        {
            return Date.ToString("d.M.yyyy", CultureInfo.InvariantCulture);
        }

        public int CompareTo(PurchaseDate other)
        {
            return Date.CompareTo(other.Date);
        }

        public static bool operator >=(PurchaseDate d1, PurchaseDate d2) => d1.Date >= d2.Date;
        public static bool operator <=(PurchaseDate d1, PurchaseDate d2) => d1.Date <= d2.Date;
        public static bool operator >(PurchaseDate d1, PurchaseDate d2) => d1.Date > d2.Date;
        public static bool operator <(PurchaseDate d1, PurchaseDate d2) => d1.Date < d2.Date;
        public static bool operator ==(PurchaseDate d1, PurchaseDate d2) => d1.Date == d2.Date;
        public static bool operator !=(PurchaseDate d1, PurchaseDate d2) => d1.Date != d2.Date;

        public override bool Equals(object obj)
        {
            return obj is PurchaseDate other && Date.Equals(other.Date);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }
    }
}