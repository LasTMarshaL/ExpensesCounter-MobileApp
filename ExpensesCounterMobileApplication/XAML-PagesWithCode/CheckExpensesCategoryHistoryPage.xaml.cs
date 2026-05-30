using ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;
using System.Collections.ObjectModel;

namespace ExpensesCounterMobileApplication; 

public partial class CheckExpensesCategoryHistoryPage : ContentPage 
{
    private string categoryNameView = ""; 
    public ObservableCollection<ExpenseViewModel> NeededExpensesList { get; set; } = new(); 
    private List<ExpenseViewModel> _allExpenses = new(); 

    public PriceFilter PriceFilterProperty { get; set; } = new ();
    public DateAndTimeFilter DateAndTimeFilterProperty { get; set; } = new ();

    public CheckExpensesCategoryHistoryPage(string categoryName) 
    {
		InitializeComponent();

        CategoryName.Text = categoryName; 
        categoryNameView = categoryName; 

        BindingContext = this; 
    }

    enum UIState 
    {
        None,       
        Menu,       
        Filter      
    }
    private UIState uIState = UIState.None;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData(categoryNameView); 

        await LoadDataForFilters(categoryNameView);
    }

    public async Task LoadData(string categoryName)
    {
        NeededExpensesList.Clear(); 
        _allExpenses.Clear(); 

        var expenses = await ExpensesDatabase.GetExpensesOfConcreteCategoryFromDatabase(categoryName); 
        _allExpenses = expenses; 

        foreach (var expense in expenses) 
        {
            NeededExpensesList.Add(expense); 
        }
    }

    public async Task LoadDataForFilters(string categoryName) 
    {
        PriceFilterProperty.PriceFrom = await ExpensesDatabase.GetTheLowestPriceOfCategoryFromDatabase(categoryName); 
        PriceFilterProperty.PriceTo = await ExpensesDatabase.GetTheHighestPriceOfCategoryFromDatabase(categoryName);

        var earliest = await ExpensesDatabase.GetEarliestDateAndTimeOfCategoryFromDatabase(categoryName); 
        var latest = await ExpensesDatabase.GetLatestDateAndTimeOfCategoryFromDatabase(categoryName); 

        DateAndTimeFilterProperty.DateFrom = earliest.Date; 
        DateAndTimeFilterProperty.TimeFrom = earliest.TimeOfDay; 

        DateAndTimeFilterProperty.DateTo = latest.Date; 
        DateAndTimeFilterProperty.TimeTo = latest.TimeOfDay; 
    }

    public async void RemoveExpenseButtonClicked(object sender, EventArgs e) 
    {
        bool answer = await DisplayAlertAsync($"Confirmation", "Remove this expense?", "Yes", "No");
        if (answer)
        {
            var button = (Button)sender; 
            var expense = (ExpenseViewModel)button.BindingContext;

            await ExpensesDatabase.RemoveExpenseFromDatabase(expense); 

            await LoadDataForFilters(categoryNameView); 

            NeededExpensesList.Remove(expense);
            _allExpenses.Remove(expense); 
        }
    }

    public async void RemoveAllExpensesButtonClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlertAsync("Confirmation", $"Remove all expenses from {CategoryName.Text}?", "Yes", "No");
        if (answer) 
        {
            await ExpensesDatabase.RemoveAllExpenses(categoryNameView); 

            PriceFilterProperty.PriceFrom = 0; 
            PriceFilterProperty.PriceTo = 0;

            DateAndTimeFilterProperty.DateFrom = DateTime.Now; 
            DateAndTimeFilterProperty.DateTo = DateTime.Now;

            NeededExpensesList.Clear(); 
            _allExpenses.Clear();
        }
    }

    public async void OpenExtraMenuButtonClicked(object sender, EventArgs e) 
    {
        if (uIState == UIState.None) 
        {
            uIState = UIState.Menu; 

            OpenFilterButton.IsVisible = true; 
            RemoveAllButton.IsVisible = true;
            ResetFiltersButton.IsVisible = true;

            BackgroundOverlay.InputTransparent = false; 

            await BackgroundOverlay.FadeToAsync(0.75, 250);

            DopMenuButton.Text = "-"; 
        }
        else
        {
            await CloseExtraMenu();
        }
    }

   
    public async void OpenFilterButtonClicked(object sender, EventArgs e) 
    {
        if (uIState == UIState.Menu) 
        {
            uIState = UIState.Filter;

            Filter.IsVisible = true; 

            RemoveAllButton.IsVisible = false;
            OpenFilterButton.IsVisible = false;
            ResetFiltersButton.IsVisible = false; 
        }
    }

    public async void ResetFiltersButtonClicked(object sender, EventArgs e)
    {
        NeededExpensesList.Clear();

        foreach (var expense in _allExpenses)
        {
            NeededExpensesList.Add(expense);
        }
    }

    public async Task CloseExtraMenu()
    {
        if (uIState == UIState.Menu)
        {
            await BackgroundOverlay.FadeToAsync(0, 250);

            BackgroundOverlay.InputTransparent = true; 

            RemoveAllButton.IsVisible = false;
            OpenFilterButton.IsVisible = false;
            ResetFiltersButton.IsVisible = false;

            DopMenuButton.Text = "+";

            uIState = UIState.None;
        }
        else if (uIState == UIState.Filter)
        {
            Filter.IsVisible = false;

            RemoveAllButton.IsVisible = true;
            OpenFilterButton.IsVisible = true;
            ResetFiltersButton.IsVisible = true;

            uIState = UIState.Menu; 
        }
    }

    public async Task CloseExtraFilterMenu()
    {
        OpenFilterButton.IsVisible = false; 
        ResetFiltersButton.IsVisible = false;
    }

    public async void FilterButtonClicked(object sender, EventArgs e) 
    {
        DateTime from = DateAndTimeFilterProperty.DateFrom.Date + DateAndTimeFilterProperty.TimeFrom; 
        DateTime to = DateAndTimeFilterProperty.DateTo.Date + DateAndTimeFilterProperty.TimeTo; 

        var filtered = _allExpenses.AsEnumerable(); 
        filtered = filtered.Where(e => e.Price >= PriceFilterProperty.PriceFrom && e.Price <= PriceFilterProperty.PriceTo && e.DateAndTime >= from && e.DateAndTime <= to); 

        NeededExpensesList.Clear(); 

        foreach (var expense in filtered) 
        {
            NeededExpensesList.Add(expense); 
        }
    }

    public async void CloseExtraMenuClickedOnBackground(object sender, EventArgs e)
    {
        await CloseExtraMenu();
    }

    
    public async void GoBackButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync(animated: false); 
    }
}