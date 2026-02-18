namespace ExpensesCounterMobileApplication; // Conect class to the main project namespace

public partial class AddExpenseToDataBasePage : ContentPage // This page is responsiable for adding expense to the database with user's data
{
	public AddExpenseToDataBasePage(string categoryName) // Consturctor, which is created with class object. It is used to set basic data and make basic actions
    {
		InitializeComponent(); // Connect XAML code with this class

		Category.Text = $"Category: {categoryName}"; // Set the name of the chousen category to the XAML page
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void GoBackButtonClicked(object? sender, EventArgs e) // This method goes back to the previous page // sender - who pressed the button, e - information of the click
    {
		await Navigation.PopAsync(animated: false); // Go to the previous XAML page without basic animation
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
	public async Task GoBackToTheMainMenu() // This method goes back to the main menu CheckExpenseCategories // sender - who pressed the button, e - information of the click
    {
        await Navigation.PushAsync(new MainPage(), animated: false); // Go to the MainPage XAML page without basic animation
    }
    
	// Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void AddExpenseButtonClicked(object? sender, EventArgs e) // This method adds expense to the database
	{
		string category = Category.Text.Replace("Category: ", ""); // Get name of the category from the XAML page

        if (!double.TryParse(EnteredPrice.Text, out double price)) // Get number from the XAML page and try to convert it into double type
        {
            await this.DisplayAlertAsync(
            "Error!",
            "Not valid price!",
            "OK"
            ); // Show window with message (title, text, button) // If can't, exeption window for user
			return; // Stop 
        }
        DateTime date = DateTime.Now; // Set date of expense (intialy current date)
        TimeSpan time = DateTime.Now.TimeOfDay; // Set time of expense (intialy current time)
        if (DatePicker.Date != null) // If input date from the XAML page is not null
		{
			date = DatePicker.Date.Value; // Set user's chousen date
		}
		if (TimePicker.Time != null) // If input time from the XAML page is not null
        {
			time = TimePicker.Time.Value; // Set user's chousen time
        }
        DateTime selectedDateAndTime = date + time; // Get date and time together

        string? comment = string.IsNullOrWhiteSpace(EnteredComment.Text) ? null : EnteredComment.Text; // comment from the XAML page // Check if it is null or not

        var expense = new Expense
		{
			Category = category,
			Price = Math.Abs(price),
			DateAndTime = selectedDateAndTime,
			Comment = comment
        }; // Create Expense model to insert into the database

        await ExpensesDataBaseScript.AddExpenseToDatabase(expense); // Insert data into the database

        await this.DisplayAlertAsync(
			"Success!",
			"Expense was added!",
			"OK"
			); // Show window with message (title, text, button) // Show, that expense was added

		await GoBackToTheMainMenu(); // Go to the MainPage XAML
	}
}