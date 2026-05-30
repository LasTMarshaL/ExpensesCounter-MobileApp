using ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExpensesCounterMobileApplication
{
    public class ExpensesDatabase
    {
        public static async Task<string> GetDatabasePath() 
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpensesDataBase.db3"); 

            if (!File.Exists(databasePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("ExpensesDataBase.db3"); 
                using var fileStream = File.Create(databasePath); 
                await stream.CopyToAsync(fileStream); 
            }

            return databasePath; 
        }


        /// <summary>
        /// Deletes the existing database file and copies a new one from the application package. ALL USER DATA WILL BE LOST!
        /// </summary>
        public async static Task UpdateDatabase()
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpensesDataBase.db3"); 

            if (File.Exists(databasePath)) 
            {
                File.Delete(databasePath); 
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync("ExpensesDataBase.db3"); 
            using var fileStream = File.Create(databasePath); 
            await stream.CopyToAsync(fileStream); 
        }

        public async static Task CreateTable() 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand(); 
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Expenses (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Category TEXT NOT NULL,
                Price REAL NOT NULL,
                DateAndTime TEXT NOT NULL,
                Comment TEXT
            );"; 

            await command.ExecuteNonQueryAsync();
        }

        public async static Task AddExpenseToDatabase(ExpenseViewModel expense) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"
                INSERT INTO Expenses (Category, Price, DateAndTime, Comment)
                VALUES ($category, $price, $dateAndTime, $comment);
            "; 

            command.Parameters.AddWithValue("category", expense.Category); 
            command.Parameters.AddWithValue("price", expense.Price); 
            command.Parameters.AddWithValue("dateAndTime", expense.DateAndTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)); 
            command.Parameters.AddWithValue("comment", (object?)expense.Comment ?? DBNull.Value); 

            await command.ExecuteNonQueryAsync();
        }


        public async static Task<List<ExpenseViewModel>> GetAllExpensesFromDatabase()
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            var dataList = new List<ExpenseViewModel>(); 

            using var connection = new SqliteConnection($"Data Source={databasePath}");    
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"
                SELECT ID, Category, Price, DateAndTime, Comment FROM Expenses
                "; 

            using var reader = await command.ExecuteReaderAsync(); 
            while(await reader.ReadAsync()) 
            {
                dataList.Add(new ExpenseViewModel
                {
                    ID = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    DateAndTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), 
                    Comment = reader.IsDBNull(4) ? null : reader.GetString(4)
                }); 
            }

            return dataList;
        }


        public async static Task<List<ExpenseViewModel>> GetExpensesOfConcreteCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            var dataList = new List<ExpenseViewModel>(); 

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"
                SELECT ID, Category, Price, DateAndTime, Comment
                FROM Expenses
                WHERE Category = $categoryName
                ";

            command.Parameters.AddWithValue("$categoryName", categoryName);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                dataList.Add(new ExpenseViewModel
                {
                    ID = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    DateAndTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    Comment = reader.IsDBNull(4) ? null : reader.GetString(4)
                });
            }

            return dataList;
        }



        public async static Task<double> GetTotalSumOfCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            double totalSum = 0;

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT SUM(Price) AS Total 
                FROM Expenses 
                WHERE Category = $categoryName
                "; 

            command.Parameters.AddWithValue("$categoryName", categoryName); 

            var result = await command.ExecuteScalarAsync(); 

            if (result != DBNull.Value && result != null) 
            {
                totalSum = Convert.ToDouble(result);
            }

            return totalSum;
        }


        public async static Task<double> GetTotalSumOfAllCategoriesFromDatabase() 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            double totalSum = 0;

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync();

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT SUM(Price) AS Total 
                FROM Expenses 
                "; 

            var result = await command.ExecuteScalarAsync();

            if (result != DBNull.Value && result != null) 
            {
                totalSum = Convert.ToDouble(result);
            }

            return totalSum; 
        }


        public async static Task<double> GetTheLowestPriceOfCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();  

            double lowestPrice = 0; 

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT MIN(Price)
                FROM Expenses 
                WHERE Category = $categoryName
                "; 

            command.Parameters.AddWithValue("$categoryName", categoryName); 

            var result = await command.ExecuteScalarAsync(); 
            if (result != DBNull.Value && result != null) 
            {
                lowestPrice = Convert.ToDouble(result); 
            }

            return lowestPrice; 
        }


        public async static Task<double> GetTheHighestPriceOfCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            double highestPrice = 0; 

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT MAX(Price)
                FROM Expenses 
                WHERE Category = $categoryName
                "; 

            command.Parameters.AddWithValue("$categoryName", categoryName); 

            var result = await command.ExecuteScalarAsync(); 

            if (result != DBNull.Value && result != null) 
            {
                highestPrice = Convert.ToDouble(result); 
            }

            return highestPrice;
        }


        public async static Task<DateTime> GetEarliestDateAndTimeOfCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            DateTime earliestDate = DateTime.Now; 

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT MIN(DateAndTime)
                FROM Expenses 
                WHERE Category = $categoryName
                "; 

            command.Parameters.AddWithValue("$categoryName", categoryName); 

            var result = await command.ExecuteScalarAsync(); 

            if (result != DBNull.Value && result != null) 
            {
                earliestDate = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); 
            }

            return earliestDate; 
        }


        public async static Task<DateTime> GetLatestDateAndTimeOfCategoryFromDatabase(string categoryName) 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            DateTime latestDateAndTime = DateTime.Now;

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT MAX(DateAndTime)
                FROM Expenses 
                WHERE Category = $categoryName
                "; 

            command.Parameters.AddWithValue("$categoryName", categoryName);

            var result = await command.ExecuteScalarAsync(); 

            if (result != DBNull.Value && result != null) 
            {
                latestDateAndTime = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); 
            }

            return latestDateAndTime;
        }

        public async static Task<double> GetTheLowestPriceOfAllCategoriesFromDatabase() 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            double lowestPrice = 0;

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            command.CommandText = @"SELECT MIN(Price)
                FROM Expenses 
                "; 

            var result = await command.ExecuteScalarAsync();

            if (result != DBNull.Value && result != null)
            {
                lowestPrice = Convert.ToDouble(result);
            }

            return lowestPrice; 
        }

        public async static Task<double> GetTheHighestPriceOfAllCategoriesFromDatabase() 
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            double highestPrice = 0; 

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT MAX(Price)
                FROM Expenses 
                "; 

            var result = await command.ExecuteScalarAsync();

            if (result != DBNull.Value && result != null) 
            {
                highestPrice = Convert.ToDouble(result);
            }

            return highestPrice;
        }

        public async static Task<DateTime> GetEarliestDateAndTimeOfAllCategoriesFromDatabase()
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            DateTime earliestDate = DateTime.Now; 

            using var connection = new SqliteConnection($"Data Source={databasePath}"); 
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT MIN(DateAndTime)
                FROM Expenses 
                "; 


            var result = await command.ExecuteScalarAsync();

            if (result != DBNull.Value && result != null) 
            {
                earliestDate = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }

            return earliestDate; 
        }

        public async static Task<DateTime> GetLatestDateAndTimeOfAllCategoriesFromDatabase()
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            DateTime latestDateAndTime = DateTime.Now;

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync(); 

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT MAX(DateAndTime)
                FROM Expenses 
                ";
           
            var result = await command.ExecuteScalarAsync(); 

            if (result != DBNull.Value && result != null) 
            {
                latestDateAndTime = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }

            return latestDateAndTime;
        }


        public async static Task RemoveExpenseFromDatabase(ExpenseViewModel removingExpense)
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath();

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Expenses 
                WHERE Id = $id";

            command.Parameters.AddWithValue("$id", removingExpense.ID); 

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        ///  Removes all expenses from the database, or only those associated with the specified category if a category name is provided.
        /// </summary>>
        public async static Task RemoveAllExpenses(string? categoryName)
        {
            string databasePath = await ExpensesDatabase.GetDatabasePath(); 

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand(); 

            if (categoryName != null)
            {
                command.CommandText = @"
                    DELETE FROM Expenses 
                    WHERE Category = $categoryName";

                command.Parameters.AddWithValue("$categoryName", categoryName); 
            }
            else
            {
                command.CommandText = @"
                    DELETE FROM Expenses";
            }

            await command.ExecuteNonQueryAsync();
        }
    }
}
