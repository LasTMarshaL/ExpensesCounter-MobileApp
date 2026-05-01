namespace ExpensesCounterMobileApplication.Database
{
    public interface IExpenseDatabase //Interface for working with the databases and tests.
    {
        Task<string> GetDatabasePath();
        Task UpdateDatabase();
        Task CreateTable();
        Task AddExpenseToDatabase(ExpenseViewModel expense);
        Task<List<ExpenseViewModel>> GetAllExpensesFromDatabase();
        Task<List<ExpenseViewModel>> GetExpensesOfConcreteCategoryFromDatabase(string categoryName);
        Task<double> GetTotalSumOfCategoryFromDatabase(string categoryName);
        Task<double> GetTotalSumOfAllCategoriesFromDatabase();
        Task<double> GetTheLowestPriceOfCategoryFromDatabase(string categoryName);
        Task<double> GetTheHighestPriceOfCategoryFromDatabase(string categoryName);
        Task<DateTime> GetEarliestDateAndTimeOfCategoryFromDatabase(string categoryName);
        Task<DateTime> GetLatestDateAndTimeOfCategoryFromDatabase(string categoryName);
        Task<double> GetTheLowestPriceOfAllCategoriesFromDatabase();
        Task<double> GetTheHighestPriceOfAllCategoriesFromDatabase();
        Task<DateTime> GetEarliestDateAndTimeOfAllCategoriesFromDatabase();
        Task<DateTime> GetLatestDateAndTimeOfAllCategoriesFromDatabase();
        Task RemoveExpenseFromDatabase(ExpenseViewModel removingExpense);
        Task RemoveAllExpenses(string? categoryName);
    }
}
