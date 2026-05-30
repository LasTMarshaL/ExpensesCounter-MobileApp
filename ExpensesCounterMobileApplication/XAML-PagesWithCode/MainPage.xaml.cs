namespace ExpensesCounterMobileApplication 
{
    public partial class MainPage : ContentPage 
    {
        public MainPage() 
        {
            InitializeComponent();

            /*Task.Run(async () => // Run asynchronus method from not asynchronus method
            {
                await ExpensesDataBaseScript.UpdateDataBase(); // Update data base. Used only in case it is needed to change database during application development
            }); */
        }


        private async void AddExpensesButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PushAsync(new AddExpensesMenuPage(), animated: false); 
        }

        private async void CheckExpensesButtonClicked(object? sender, EventArgs e) 
        {
            await Navigation.PushAsync(new CheckExpensesMenuPage(), animated: false); 
        }
    }
}
