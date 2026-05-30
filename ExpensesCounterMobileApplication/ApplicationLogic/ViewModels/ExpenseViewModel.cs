namespace ExpensesCounterMobileApplication.ApplicationLogic.ViewModels
{
    public class ExpenseViewModel 
    {
        public int ID { get; set; } 

        public string Category { get; set; } = string.Empty; 

        public double Price { get; set; } 

        public DateTime DateAndTime { get; set; } = DateTime.Today; 

        public string? Comment { get; set; }
    }
}