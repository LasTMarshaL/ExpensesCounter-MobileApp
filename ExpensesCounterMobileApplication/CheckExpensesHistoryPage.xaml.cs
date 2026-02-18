using System.Collections.ObjectModel;

namespace ExpensesCounterMobileApplication; // Conect class to the main project namespace

public partial class CheckExpensesHistoryPage : ContentPage // This page is responsiable for showing history of the chosen category
{
    public ObservableCollection<Expense> NeededExpensesList { get; set; } = new(); // List to collect needed expenses // ObservableCollection immediately sends data to the XAML code after eveach changing
    private List<Expense> _allExpenses = new(); // List with all expenses // It keeps all expenses to not lose them during filtering operation

    public PriceFilter PriceFilterProperty { get; set; } = new(); // Property of the filter by price to work with Bindig
    public DateAndTimeFilter DateAndTimeFilterProperty { get; set; } = new(); // Property of the filter by date to work with Bindig

    public CheckExpensesHistoryPage() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
    {
        InitializeComponent(); // Connect XAML code with this class

        BindingContext = this; // Is used to make XAML code see data from CategoriesViewModel class
    }

    enum UIState // Which element of the extra menu is opened
    {
        None,
        Menu,
        Filter
    }
    private UIState uIState = UIState.None;  // Set state "Nothing is  opened"

    protected override async void OnAppearing() // This method runs after loading current XAML content page (was created automaticaly) // override - change this method
    {
        base.OnAppearing(); // Save previous code of this method
        await LoadData(); // Load data from the database

        await LoadDataForFilters(); // Load date from the database for filters
    }

    public async Task LoadData() // This method loads data from the database
    {
        NeededExpensesList.Clear(); // Clear the list with showing expenses
        _allExpenses.Clear(); // Clear the list with all expenses

        var expenses = await ExpensesDataBaseScript.GetAllExpensesFromDatabase(); // Get expenses of the chosen category from the database
        _allExpenses = expenses; // Set got expenses to the list with all expenses

        foreach (var expense in expenses) // Iterate got expenses one by 1 // NeededExpnsesList = expenses - it is not possible, because NeededExpensesList is ObservableCollection
        {
            NeededExpensesList.Add(expense); // Add each expense to the list of showing expenses
        }
    }

    public async Task LoadDataForFilters() // This methods loads data for filters from the database
    {
        PriceFilterProperty.PriceFrom = await ExpensesDataBaseScript.GetTheLowestPriceOfAllCategoriesFromDatabase(); // Load initial (the lowest) price from the database
        PriceFilterProperty.PriceTo = await ExpensesDataBaseScript.GetTheHighestPriceOfAllCategoriesFromDatabase(); // Load final (the highest) price from the database

        var earliest = await ExpensesDataBaseScript.GetEarliestDateAndTimeOfAllCategoriesFromDatabase(); // Load initial (the earliest) date and time from the database 
        var latest = await ExpensesDataBaseScript.GetLatestDateAndTimeOfAllCategoriesFromDatabase(); // Load final (the latest) date and time from the database 

        DateAndTimeFilterProperty.DateFrom = earliest.Date; // Get initial (the earilest) date
        DateAndTimeFilterProperty.TimeFrom = earliest.TimeOfDay; // Get final (the latest) date

        DateAndTimeFilterProperty.DateTo = latest.Date; // Get initial (the earilest) time
        DateAndTimeFilterProperty.TimeTo = latest.TimeOfDay; // Get final (the latest) time
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void RemoveExpenseButtonClicked(object sender, EventArgs e) // This method removes expense from database // sender - who pressed the button, e - information of the click
    {
        bool answer = await DisplayAlertAsync($"Confirmation", "Remove this expense?", "Yes", "No"); // Ask user to confirm removing
        if (answer) // If user confirmed removing
        {
            var button = (Button)sender; // Get clicked button
            var expense = (Expense)button.BindingContext; // Get object Expense, which is connected with button

            await ExpensesDataBaseScript.RemoveExpenseFromDatabase(expense); // Remove expense from the database

            await LoadDataForFilters(); // Update data for filters

            NeededExpensesList.Remove(expense); // Remove expense from collection, which is connected with XAML page
            _allExpenses.Remove(expense); // Remove expense from list with all expenses
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void RemoveAllExpensesButtonClicked(object sender, EventArgs e) // This method removes expense from database // sender - who pressed the button, e - information of the click
    {
        bool answer = await DisplayAlertAsync("Confirmation", $"Remove all expenses?", "Yes", "No"); // Adk user to confirm removing
        if (answer) // If user confirmed removing
        {
            await ExpensesDataBaseScript.RemoveAllExpenses(null); // Remove all expenses from the database

            PriceFilterProperty.PriceFrom = 0; // Recet initial (the lowest) price from the database
            PriceFilterProperty.PriceTo = 0; // Recet final (the highest) price from the database

            DateAndTimeFilterProperty.DateFrom = DateTime.Now; // Recet initial (the earliest) date and time from the database 
            DateAndTimeFilterProperty.DateTo = DateTime.Now; // Recet final (the latest) date and time from the database 

            NeededExpensesList.Clear(); // Clear the list with showing expenses
            _allExpenses.Clear(); // Clear the list all expensess
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void OpenExtraMenuButtonClicked(object sender, EventArgs e) // This method opens extra menu with filters and "total" removing // sender - who pressed the button, e - information of the click
    {
        if (uIState == UIState.None) // If the extra menu is not opened
        {
            uIState = UIState.Menu; // Set state "The extra menu is opened"

            OpenFilterButton.IsVisible = true; // Show the button of the filter 
            RemoveAllButton.IsVisible = true; // Show the button of removing all expenses
            ResetFiltersButton.IsVisible = true; // Show the button of reseting filters

            BackgroundOverlay.InputTransparent = false; // Make buttons inaccessible behind extra menu

            await BackgroundOverlay.FadeToAsync(0.75, 250); // Darken background by 75% for 250 miliseconds

            DopMenuButton.Text = "-"; // Change text on the button to "-"
        }
        else // If the extra menu is opened
        {
            await CloseExtraMenu();// Close the extra menu
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void OpenFilterButtonClicked(object sender, EventArgs e) //This method opens menu of the filter by price // sender - who pressed the button, e - information of the click
    {
        if (uIState == UIState.Menu) // If the extra menu is opened
        {
            uIState = UIState.Filter; // Set state "Filter is opened"

            Filter.IsVisible = true; // Show the filter by price

            RemoveAllButton.IsVisible = false; // Hide the button of the menu of remiving all expenses
            OpenFilterButton.IsVisible = false; // Show the button of the filter 
            ResetFiltersButton.IsVisible = false; // Hide the button of the resetting the filters
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void ResetFiltersButtonClicked(object sender, EventArgs e) // sender - who pressed the button, e - information of the click
    {
        NeededExpensesList.Clear(); // Clear the list of expenses showing on XAML page

        foreach (var expense in _allExpenses) // Iterate allt expenses one by 1
        {
            NeededExpensesList.Add(expense); // Add each expense to the list of showing expenses
        }
    }

    public async Task CloseExtraMenu()  // This method closes the extra menu
    {
        if (uIState == UIState.Menu) // If the extra menu is opened
        {
            await BackgroundOverlay.FadeToAsync(0, 250); // Darken background by 0% for 250 miliseconds

            BackgroundOverlay.InputTransparent = true; // Make buttons accessible behind extra menu

            RemoveAllButton.IsVisible = false; // Hide the button of the menu of remiving all expenses
            OpenFilterButton.IsVisible = false; // Show the button of the filter b
            ResetFiltersButton.IsVisible = false; // Hide the button of the resetting the filters

            DopMenuButton.Text = "+"; // Change text on the button to "-"

            uIState = UIState.None; // Set state "Nothing is  opened"
        }
        else if (uIState == UIState.Filter) // If some filter is opened
        {
            Filter.IsVisible = false; // Hide the filter

            RemoveAllButton.IsVisible = true; // Show the button of the menu of remiving all expenses
            OpenFilterButton.IsVisible = true; // Show the button of the filter by
            ResetFiltersButton.IsVisible = true; // Hide the button of the resetting the filters

            uIState = UIState.Menu; // Set state "The extra menu is opened"
        }
    }

    public async Task CloseExtraFilterMenu() // This method closes the extra menu
    {
        OpenFilterButton.IsVisible = false; // Hide the button of the filter
        ResetFiltersButton.IsVisible = false; // Hide the button of reseting filters
    }

    // Reminder: var can be used, because type is known for sure
    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void FilterButtonClicked(object sender, EventArgs e) // This methods filters expenses by input parameters of price // sender - who pressed the button, e - information of the click
    {
        DateTime from = DateAndTimeFilterProperty.DateFrom.Date + DateAndTimeFilterProperty.TimeFrom; // Unite the earliest entered date and time
        DateTime to = DateAndTimeFilterProperty.DateTo.Date + DateAndTimeFilterProperty.TimeTo; // Unite the earliest entered date and time

        var filtered = _allExpenses.AsEnumerable(); // Makes compiler see elements of this this list as link one by one 
        filtered = filtered.Where(e => e.Price >= PriceFilterProperty.PriceFrom && e.Price <= PriceFilterProperty.PriceTo && e.DateAndTime >= from && e.DateAndTime <= to); // Get values where lymbda function returns true // => - lambda

        NeededExpensesList.Clear(); // Clear list of expenses showing on XAML page

        foreach (var expense in filtered) // Iterate got expenses one by 1
        {
            NeededExpensesList.Add(expense); // Add each expense to the list of showing expenses
        }
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void CloseExtraMenuClickedOnBackground(object sender, EventArgs e) //This method closes extra menu in case of clicks on background // sender - who pressed the button, e - information of the click
    {
        await CloseExtraMenu(); // Close the extra menu
    }

    // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
    public async void GoBackButtonClicked(object? sender, EventArgs e) // This method goes back to the previous page // sender - who pressed the button, e - information of the click
    {
        await Navigation.PopAsync(animated: false); // Go to the previous XAML page without basic animation
    }
}