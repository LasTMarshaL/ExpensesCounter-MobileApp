

namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public class Expense // This class is just a model (data structure) for data base
    {
        public int ID { get; set; } // Expense unique ID

        public string Category { get; set; } = string.Empty; // Category of expense

        public double Price { get; set; } // Price (money spend)

        public DateTime DateAndTime { get; set; } = DateTime.Today; // Date and time where expense was

        public string? Comment { get; set; } // Personal user's comment
    }
}