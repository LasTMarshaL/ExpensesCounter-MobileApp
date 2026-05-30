
using ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;

namespace ExpensesCounterMobileApplication 
{
    public partial class AddExpensesMenuPage : ContentPage
    {
        public AddExpensesMenuPage()
        {
            InitializeComponent();

            BindingContext = new ExpensesCategoriesViewModel();
        }
        
        public async void CategoryClicked(object? sender, EventArgs e) 
        {
           
            if (sender is Border border && border.BindingContext is ExpensesCategoryViewModel category && category.Name != null) 
            {
                string categoryName = category.Name; 

                await Navigation.PushAsync(new AddExpenseToDataBasePage(categoryName), animated: false);
            }
        }

        public async void GoBackToTheMainPageButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PopAsync(animated: false);
        }
    }
}
