using ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;

namespace ExpensesCounterMobileApplication; 

public partial class CheckExpenseCategoriesPage : ContentPage
{
    private ExpensesCategoriesViewModel categories = new ExpensesCategoriesViewModel(); 

    public CheckExpenseCategoriesPage() 
    {
		InitializeComponent(); 

        BindingContext = categories;
    }
   protected override async void OnAppearing() 
    {
        base.OnAppearing(); 
        await LoadData(categories);
    }


    public async Task LoadData(ExpensesCategoriesViewModel categories)
    {

        for (int i = 0; i < categories.Categories.Count; i++) 
        {
            var category = categories.Categories[i]; 

            if (!string.IsNullOrEmpty(category.Name)) 
            {
                categories.Categories[i].TotalSum = await ExpensesDatabase.GetTotalSumOfCategoryFromDatabase(category.Name); 
            }
        }
    }

    public async void CategoryClicked(object? sender, EventArgs e)
    {
        if (sender is Border border && border.BindingContext is ExpensesCategoryViewModel category && category.Name != null)
        {
            string categoryName = category.Name;

            await Navigation.PushAsync(new CheckExpensesCategoryHistoryPage(categoryName), animated: false);
        }
    }

    public async void GoBackButtonClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync(animated: false);
    }
}