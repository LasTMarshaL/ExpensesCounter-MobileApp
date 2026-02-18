using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExpensesCounterMobileApplication // // Conect class to the main project namespace
{
    public class ExpensesDataBaseScript // This class is responsiable for creating and working with data base
    {
        public static async Task<string> GetDatabasePath() // This method copies database from main package to the folder of the application, where application can edit it
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpensesDataBase.db3"); // Find suitable folder and make path

            if (!File.Exists(databasePath)) // If there is not database there
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("ExpensesDataBase.db3"); // Open database file from the main package
                using var fileStream = File.Create(databasePath); // Create database file
                await stream.CopyToAsync(fileStream); // Copy database to the local folder of the application
            }

            return databasePath; // Return path to the local database of the application
        }

        public async static Task UpdateDatabase() // This method updates database in the folder of application (delete old and set new)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpensesDataBase.db3"); // Find suitable folder and make path

            if (File.Exists(databasePath)) // If there is database there
            {
                File.Delete(databasePath); // Delete all database
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync("ExpensesDataBase.db3"); // Open database file from the main package
            using var fileStream = File.Create(databasePath); // Create database file
            await stream.CopyToAsync(fileStream); // Copy database to the local folder of the application
        }

        public async static Task CreateTable() // This method creates database table if it does not exist
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Expenses (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Category TEXT NOT NULL,
                Price REAL NOT NULL,
                DateAndTime TEXT NOT NULL,
                Comment TEXT
            );"; // SQL-request (we use "@" instead of using real values to awoid SQL-injections)

            await command.ExecuteNonQueryAsync(); // Execute SQL-request without any feedback
        }

        public async static Task AddExpenseToDatabase(Expense expense) // This method adds expense to the database
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"
                INSERT INTO Expenses (Category, Price, DateAndTime, Comment)
                VALUES ($category, $price, $dateAndTime, $comment);
            "; // SQL-request (we use "@" instead of using real values to awoid SQL-injections)

            command.Parameters.AddWithValue("category", expense.Category); // Bind the value of the category to the parameter of the SQL-request
            command.Parameters.AddWithValue("price", expense.Price); // Bind the value of the category to the parameter of the SQL-request
            command.Parameters.AddWithValue("dateAndTime", expense.DateAndTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)); // Bind the value of the category to the parameter of the SQL-request
            command.Parameters.AddWithValue("comment", (object?)expense.Comment ?? DBNull.Value); // Bind the value of the category to the parameter of the SQL-request

            await command.ExecuteNonQueryAsync(); // Execute SQL-request without any feedback
        }

        public async static Task<List<Expense>> GetAllExpensesFromDatabase() // This method gets all data from the database
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            var dataList = new List<Expense>(); // List for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"
                SELECT ID, Category, Price, DateAndTime, Comment FROM Expenses"; // SQL-request

            using var reader = await command.ExecuteReaderAsync(); // Create SQL-request to the database to get data
            while(await reader.ReadAsync()) // Go to the next string of the database while there strings
            {
                dataList.Add(new Expense
                {
                    ID = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    DateAndTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), // Convert string to DateTime with culture defirences
                    Comment = reader.IsDBNull(4) ? null : reader.GetString(4)
                }); // Add data from the database using its index  
            }

            return dataList; // Return received data as a list
        }

        public async static Task<List<Expense>> GetExpensesOfConcreteCategoryFromDatabase(string categoryName) // This method gets expense only for concrete from the database
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            var dataList = new List<Expense>(); // List for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"
                SELECT ID, Category, Price, DateAndTime, Comment
                FROM Expenses
                WHERE Category = $categoryName
                "; // SQL-request

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            using var reader = await command.ExecuteReaderAsync(); // Create SQL-request to the database to get data
            while (await reader.ReadAsync()) // Go to the next string of the database while there strings
            {
                dataList.Add(new Expense
                {
                    ID = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    DateAndTime = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture), // Convert string to DateTime with culture defirences
                    Comment = reader.IsDBNull(4) ? null : reader.GetString(4)
                }); // Add data from the database using its index  
            }

            return dataList; // Return received data as a list
        }

        public async static Task<double> GetTotalSumOfCategoryFromDatabase(string categoryName) // This method gets total sum of the expneses in one category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double totalSum = 0; // Doublenumber for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT SUM(Price) AS Total 
                FROM Expenses 
                WHERE Category = $categoryName
                "; // SQL-request

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                totalSum = Convert.ToDouble(result); // Covert result to double
            }

            return totalSum; // Return received data as a double number
        }

        public async static Task<double> GetTotalSumOfAllCategoriesFromDatabase() // This method gets total sum of the expneses in all categories
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double totalSum = 0; // Doublenumber for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT SUM(Price) AS Total 
                FROM Expenses 
                "; // SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                totalSum = Convert.ToDouble(result); // Covert result to double
            }

            return totalSum; // Return received data as a double number
        }

        public async static Task<double> GetTheLowestPriceOfCategoryFromDatabase(string categoryName) // This method gets the lowest price of the expneses of one category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double lowestPrice = 0; // Double number for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MIN(Price)
                FROM Expenses 
                WHERE Category = $categoryName
                "; // SQL-request

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                lowestPrice = Convert.ToDouble(result); // Covert result to double
            }

            return lowestPrice; // Return received data as a double number
        }

        public async static Task<double> GetTheHighestPriceOfCategoryFromDatabase(string categoryName) // This method gets the highest price of the expneses of one category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double highestPrice = 0; // Double number for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MAX(Price)
                FROM Expenses 
                WHERE Category = $categoryName
                "; // SQL-request

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                highestPrice = Convert.ToDouble(result); // Covert result to double
            }

            return highestPrice; // Return received data as a double number
        }

        public async static Task<DateTime> GetEarliestDateAndTimeOfCategoryFromDatabase(string categoryName) // This method gets the earliest date of the expneses of one category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            DateTime earliestDate = DateTime.Now; // DateTime variable for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MIN(DateAndTime)
                FROM Expenses 
                WHERE Category = $categoryName
                "; // SQL-request (Format "yyyy-MM-dd HH:mm" of DateAndTime allows you Min/Max in SQL-request despite it is keeped as a string)

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                earliestDate = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); // Convert string to DateTime with culture defirences // ! - 100% not null
            }

            return earliestDate; // Return received data as a date and time
        }

        public async static Task<DateTime> GetLatestDateAndTimeOfCategoryFromDatabase(string categoryName) // This method gets the earliest date of the expneses of one category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            DateTime latestDateAndTime = DateTime.Now; // DateTime variable for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MAX(DateAndTime)
                FROM Expenses 
                WHERE Category = $categoryName
                "; // SQL-request (Format "yyyy-MM-dd HH:mm" of DateAndTime allows you Min/Max in SQL-request despite it is keeped as a string)

            command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                latestDateAndTime = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); // Convert string to DateTime with culture defirences // ! - 100% not null
            }

            return latestDateAndTime; // Return received data as a date and time
        }

        public async static Task<double> GetTheLowestPriceOfAllCategoriesFromDatabase() // This method gets the lowest price of the expneses of all categories
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double lowestPrice = 0; // Double number for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MIN(Price)
                FROM Expenses 
                "; // SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                lowestPrice = Convert.ToDouble(result); // Covert result to double
            }

            return lowestPrice; // Return received data as a double number
        }

        public async static Task<double> GetTheHighestPriceOfAllCategoriesFromDatabase() // This method gets the highest price of the expneses of all categories
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            double highestPrice = 0; // Double number for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MAX(Price)
                FROM Expenses 
                "; // SQL-request

            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                highestPrice = Convert.ToDouble(result); // Covert result to double
            }

            return highestPrice; // Return received data as a double number
        }

        public async static Task<DateTime> GetEarliestDateAndTimeOfAllCategoriesFromDatabase() // This method gets the earliest date of the expneses of all categories
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            DateTime earliestDate = DateTime.Now; // DateTime variable for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MIN(DateAndTime)
                FROM Expenses 
                "; // SQL-request (Format "yyyy-MM-dd HH:mm" of DateAndTime allows you Min/Max in SQL-request despite it is keeped as a string)


            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                earliestDate = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); // Convert string to DateTime with culture defirences // ! - 100% not null
            }

            return earliestDate; // Return received data as a date and time
        }

        public async static Task<DateTime> GetLatestDateAndTimeOfAllCategoriesFromDatabase() // This method gets the earliest date of the expneses of all categories
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            DateTime latestDateAndTime = DateTime.Now; // DateTime variable for data from the table of the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"SELECT MAX(DateAndTime)
                FROM Expenses 
                "; // SQL-request (Format "yyyy-MM-dd HH:mm" of DateAndTime allows you Min/Max in SQL-request despite it is keeped as a string)
           
            var result = await command.ExecuteScalarAsync(); // Get result value as the first string received from the table of the database

            if (result != DBNull.Value && result != null) // If there is data in the table of the database and it in not null
            {
                latestDateAndTime = DateTime.ParseExact(result.ToString()!, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture); // Convert string to DateTime with culture defirences // ! - 100% not null
            }

            return latestDateAndTime; // Return received data as a date and time
        }

        public async static Task RemoveExpenseFromDatabase(Expense removingExpense) // This method removes expense from the database
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            using var connection = new SqliteConnection($"Data Source={databasePath}"); // Create connection to the database (using controls closing connection without any problems or extra commands)
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request
            command.CommandText = @"
                DELETE FROM Expenses 
                WHERE Id = $id"; // SQL-request

            command.Parameters.AddWithValue("$id", removingExpense.ID); // Bind the value of the category to the parameter of the SQL-request

            await command.ExecuteNonQueryAsync(); // Execute SQL-request without any feedback
        }

        public async static Task RemoveAllExpenses(string? categoryName) // This method removes all expenses from database / category
        {
            string databasePath = await ExpensesDataBaseScript.GetDatabasePath(); // Get file path to the database

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            await connection.OpenAsync(); // Open connection to the database (using will close it automaticly)

            var command = connection.CreateCommand(); // Create command to execute SQL-request

            if (categoryName != null) // if category was inputed
            {
                command.CommandText = @"
                    DELETE FROM Expenses 
                    WHERE Category = $categoryName"; // SQL-request

                command.Parameters.AddWithValue("$categoryName", categoryName); // Bind the value of the category to the parameter of the SQL-request
            }
            else
            {
                command.CommandText = @"
                    DELETE FROM Expenses"; // SQL-request
            }

            await command.ExecuteNonQueryAsync(); // Execute SQL-request without any feedback
        }
    }
}
