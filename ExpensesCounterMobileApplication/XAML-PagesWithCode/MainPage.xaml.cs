namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public partial class MainPage : ContentPage // This class is responsiable for working with MainPage XAML content page
    {
        public MainPage() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
        {
            InitializeComponent(); // Connect XAML code with this class

            /*Task.Run(async () => // Run asynchronus method from not asynchronus method
            {
                await ExpensesDataBaseScript.UpdateDataBase(); // Update data base. Used only in case it is needed to change database during application development
            }); */
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        private async void AddExpensesButtonClicked(object? sender, EventArgs e) // This method goes to the AddExpensesButtonClicked XAML page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PushAsync(new AddExpensesMenuPage(), animated: false); // Go to AddExpensesMenuPage XAML page without basic animation
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        private async void CheckExpensesButtonClicked(object? sender, EventArgs e) // This method goes to the CheckExpensesButtonClicked XAML page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PushAsync(new CheckExpensesMenuPage(), animated: false); // Go to CheckExpensesMenuPage XAML page without basic animation
        }
    }
}
