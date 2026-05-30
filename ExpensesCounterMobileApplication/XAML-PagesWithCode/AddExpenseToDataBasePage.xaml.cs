using ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;

namespace ExpensesCounterMobileApplication;

public partial class AddExpenseToDataBasePage : ContentPage
{
	public AddExpenseToDataBasePage(string categoryName)
    {
		InitializeComponent();

		Category.Text = $"Category: {categoryName}";
    }


    public async void GoBackButtonClicked(object? sender, EventArgs e)
    {
		await Navigation.PopAsync(animated: false);
    }


	public async Task GoBackToTheMainMenu() 
    {
        await Navigation.PushAsync(new MainPage(), animated: false); 
    }
    

    public async void AddExpenseButtonClicked(object? sender, EventArgs e) 
	{
		string category = Category.Text.Replace("Category: ", ""); 

        if (!double.TryParse(EnteredPrice.Text, out double price)) 
        {
            await this.DisplayAlertAsync(
            "Error!",
            "Not valid price!",
            "OK"
            );
			return; 
        }
        DateTime date = DateTime.Now; 
        TimeSpan time = DateTime.Now.TimeOfDay; 
        if (DatePicker.Date != null) 

		{
			date = DatePicker.Date.Value; 
		}
		if (TimePicker.Time != null) 
        {
			time = TimePicker.Time.Value;
        }
        DateTime selectedDateAndTime = date + time; 

        string? comment = string.IsNullOrWhiteSpace(EnteredComment.Text) ? null : EnteredComment.Text; 

        var expense = new ExpenseViewModel
		{
			Category = category,
			Price = Math.Abs(price),
			DateAndTime = selectedDateAndTime,
			Comment = comment
        }; 

        await ExpensesDatabase.AddExpenseToDatabase(expense); 

        await this.DisplayAlertAsync(
			"Success!",
			"Expense was added!",
			"OK"
			); 

		await GoBackToTheMainMenu();
	}
}