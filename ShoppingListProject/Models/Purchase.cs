namespace ShoppingListProject.Models
{
    public class Purchase
    {
        public string Name { get; set; }

        public string Comment { get; set; }

        public decimal Credits { get; set; }

        public PurchaseDate Date { get; set; }

        public override string ToString()
        {
            return $"  Name: {Name}\n  Comment: {Comment}\n  Amount: {Credits:F2} $\n  Date: {Date}";
        }
    }
}