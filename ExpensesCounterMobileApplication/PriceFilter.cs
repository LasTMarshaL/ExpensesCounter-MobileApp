
using System.ComponentModel;

namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public class PriceFilter: INotifyPropertyChanged // This class is responsiable for keeping model of filter by price
    {
        private double _priceFrom = 0; // The lowest price
        public double PriceFrom // Property to get acces to the lowest price
        {
            get
            {
                return _priceFrom; // Return the lowest price
            }
            set
            {
                if (_priceFrom != value) // If the lowest price is not equal to some value
                {
                    _priceFrom = value;
                    OnPropertyChanged(nameof(PriceFrom)); // Say UI, that property was changed
                }
            }
        }
        private double _priceTo = 0;  // The highest price
        public double PriceTo // Property to get acces to the highest price
        {
            get
            {
                return _priceTo; // Return the highest price
            }
            set
            {
                if (_priceTo != value) // If the highest price is not equal to some value
                {
                    _priceTo = value; 
                    OnPropertyChanged(nameof(PriceTo)); // Say UI, that property was changed
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Event, which is executed after changing of property
        protected void OnPropertyChanged(string propertyName) // This method sends information to UI (XAML page), that property was changed
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Send information to UI (XAML page), that property was changed
        }
    }
}
