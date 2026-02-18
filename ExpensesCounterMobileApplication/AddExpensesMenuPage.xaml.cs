
namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public partial class AddExpensesMenuPage : ContentPage // This class is responsiable for working with AddExpensesMenuPage XAML page
    {
        public AddExpensesMenuPage() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
        {
            InitializeComponent(); // Connect XAML code with this class

            BindingContext = new CategoriesViewModel(); // Is used to make XAML code see data from CategoriesViewModel class
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        public async void CategoryClicked(object? sender, EventArgs e) // This method goes to the AddExpenseToDataBase XAMl page with name of the clicked category // sender - who pressed the button, e - information of the click
        {
            // If category button was clicked (border type is used for buttons there) and border type is instanse of needed class
            if (sender is Border border && border.BindingContext is ExpensesCategory category && category.Name != null) // Such checking is used, because this method can be connected with a lot of objects
            {
                string categoryName = category.Name; // Get the name of the chousen category

                await Navigation.PushAsync(new AddExpenseToDataBasePage(categoryName), animated: false); // Go to AddExpenseToDataBase XAML page with received name of the chousen category without basic animation
            }
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        public async void GotoBackToTheMainPageButtonClicked(object? sender, EventArgs e) // This method goes  back to the previous page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PopAsync(animated: false); // Go to prevoius XAML page without basic animation
        }
    }
}
