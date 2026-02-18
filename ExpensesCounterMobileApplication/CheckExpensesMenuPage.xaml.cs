namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public partial class CheckExpensesMenuPage : ContentPage // This class is responsiable for working with CheckExpensesMenuPage XAML page
    {
        public CheckExpensesMenuPage() // Consturctor, which is created with class object. It is used to set basic data and make basic actions
        {
            InitializeComponent(); // Connect XAML code with this class
            OnAppearing(); // Start loading data from the database
        }

        protected override async void OnAppearing() // This method runs after loading current XAML content page (was created automaticaly) // override - change this method
        {
            base.OnAppearing(); // Save previous code of this method
            await LoadData(); // Load data from the database
        }

        public async Task LoadData() // This method loads data from the database to the XAML content page
        {
            double totalSum = await ExpensesDataBaseScript.GetTotalSumOfAllCategoriesFromDatabase(); // Get total sum of all categories from the database
            TotalExpensesSum.Text = $"Total sum of expenses: {totalSum.ToString()}"; // Set total sum as text at the XAML content page
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        public async void GoBackButtonClicked(object? sender, EventArgs e) // This method hoes back to the previous page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PopAsync(animated: false); // Go to the previous XAML page without basic animation
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        public async void CheckCategoriesButtonClicked(object? sender, EventArgs e) // This method goes to the CheckExpenseCategories XAML page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PushAsync(new CheckExpenseCategoriesPage(), animated: false); // Go to the CheckExpenseCategories XAML page without basic animation
        }

        // Asynchronous method is used to make program wait for changing page without block of the interface (void is used for UI events, in other cases Task is used)
        public async void CheckHistoryButtonClicked(object? sender, EventArgs e) // This method goes to the CheckExpenseHistory XAML page // sender - who pressed the button, e - information of the click
        {
            await Navigation.PushAsync(new CheckExpensesHistoryPage(), animated: false); // Go to the CheckExpenseCategories XAML page without basic animation
        }
    }
}

