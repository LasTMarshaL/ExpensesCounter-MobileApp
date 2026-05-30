using System.Collections.ObjectModel;

namespace ExpensesCounterMobileApplication.ApplicationLogic.ViewModels
{
    public class ExpensesCategoriesViewModel
    {
        public ObservableCollection<ExpensesCategoryViewModel> Categories { get; set; } = new ObservableCollection<ExpensesCategoryViewModel>(); 

        public ExpensesCategoriesViewModel() 
        {
            Categories.Add(new ExpensesCategoryViewModel { Name = "Food", Icon = "expenses_icon_food.png" });
            Categories.Add(new ExpensesCategoryViewModel { Name = "Health", Icon = "expenses_icon_health.png" });
            Categories.Add(new ExpensesCategoryViewModel { Name = "Transport", Icon = "expenses_icon_transport.png" });
            Categories.Add(new ExpensesCategoryViewModel { Name = "Entertainment", Icon = "expenses_icon_entertainment.png" });
            Categories.Add(new ExpensesCategoryViewModel { Name = "Clothes", Icon = "expenses_icon_clothes.png" });
            Categories.Add(new ExpensesCategoryViewModel { Name = "Other", Icon = "expenses_icon_other.png" });
        }
    }
}