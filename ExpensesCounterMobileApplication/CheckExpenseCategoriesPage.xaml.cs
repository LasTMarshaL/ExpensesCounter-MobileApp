namespace ExpensesCounterMobileApplication; // Conect class to the main project namespace

public partial class CheckExpenseCategoriesPage : ContentPage // This page is responsiable for showing categories, which history user wants to check
{
    private CategoriesViewModel categories = new CategoriesViewModel(); // View model of the categories to use in methods

    public CheckExpenseCategoriesPage() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
    {
		InitializeComponent(); // Connect XAML code with this class

        BindingContext = categories; // Is used to make XAML code see data from CategoriesViewModel class
    }
   protected override async void OnAppearing() // This method runs after loading current XAML content page (was created automaticaly) // override - change this method
    {
        base.OnAppearing(); // Save previous code of this method
        await LoadData(categories); // Load data from the database
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async Task LoadData(CategoriesViewModel categories) // This method loads data from the database to the XAML content page
    {

        for (int i = 0; i < categories.Categories.Count; i++) // Execute models 1 by 1
        {
            var category = categories.Categories[i]; // Current category

            if (!string.IsNullOrEmpty(category.Name)) // If name of this category is not null or empty
            {
                categories.Categories[i].TotalSum = await ExpensesDataBaseScript.GetTotalSumOfCategoryFromDatabase(category.Name); // Add total sum to this model from the database
            }
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void CategoryClicked(object? sender, EventArgs e) // This method goes to the AddExpenseToDataBase XAMl page with name of the clicked category // sender - who pressed the button, e - information of the click
    {
        // If category button was clicked (border type is used for buttons there) and border type is instanse of needed class
        if (sender is Border border && border.BindingContext is ExpensesCategory category && category.Name != null) // Such checking is used, because this method can be connected with a lot of objects
        {
            string categoryName = category.Name; // Get the name of the chousen category

            await Navigation.PushAsync(new CheckExpensesCategoryHistoryPage(categoryName), animated: false); // Go to AddExpenseToDataBase XAML page with received name of the chousen category without basic animation
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void GoBackButtonClicked(object? sender, EventArgs e) // This method goes back to the previous page
    {
        await Navigation.PopAsync(animated: false); // Go to the previous XAML page without basic animation
    }
}