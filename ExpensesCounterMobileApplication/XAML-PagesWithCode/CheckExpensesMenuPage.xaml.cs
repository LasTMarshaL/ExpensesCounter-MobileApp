namespace ExpensesCounterMobileApplication
{
    public partial class CheckExpensesMenuPage : ContentPage 
    {
        public CheckExpensesMenuPage() 
        {
            InitializeComponent();
            OnAppearing(); 
        }

        protected override async void OnAppearing() 
        {
            base.OnAppearing(); 
            await LoadData(); 
        }

        public async Task LoadData()
        {
            double totalSum = await ExpensesDatabase.GetTotalSumOfAllCategoriesFromDatabase(); 
            TotalExpensesSum.Text = $"Total sum of expenses: {totalSum.ToString()}"; 
        }

        public async void GoBackButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PopAsync(animated: false); 
        }

        public async void CheckCategoriesButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PushAsync(new CheckExpenseCategoriesPage(), animated: false); 
        }

        public async void CheckHistoryButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PushAsync(new CheckExpensesHistoryPage(), animated: false); 
        }
    }
}

