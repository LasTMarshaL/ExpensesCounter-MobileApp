
using System.Collections.ObjectModel;

namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public class CategoriesViewModel // This class is responsiable for keeping and separate data from UI and use initialization during project build in XAML code
    {
        public ObservableCollection<ExpensesCategory> Categories { get; set; } = new ObservableCollection<ExpensesCategory>(); // Initialize list with expenses categories 

        public CategoriesViewModel() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
        {
            Categories.Add(new ExpensesCategory { Name = "Food", Icon = "expenses_icon_food.png" }); // Add category with Name and Icon
            Categories.Add(new ExpensesCategory { Name = "Health", Icon = "expenses_icon_health.png" }); // Add category with Name and Icon
            Categories.Add(new ExpensesCategory { Name = "Transport", Icon = "expenses_icon_transport.png" }); // Add category with Name and Icon
            Categories.Add(new ExpensesCategory { Name = "Entertainment", Icon = "expenses_icon_entartainment.png" }); // Add category with Name and Icon
            Categories.Add(new ExpensesCategory { Name = "Clothes", Icon = "expenses_icon_clothes.png" }); // Add category with Name and Icon
            Categories.Add(new ExpensesCategory { Name = "Other", Icon = "expenses_icon_other.png" }); // Add category with Name and Icon
        }
    }
}