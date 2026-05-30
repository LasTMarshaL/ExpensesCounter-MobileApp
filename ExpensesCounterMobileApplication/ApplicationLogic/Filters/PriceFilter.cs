
using System.ComponentModel;

namespace ExpensesCounterMobileApplication
{
    public class PriceFilter: INotifyPropertyChanged
    {
        private double _priceFrom = 0;
        public double PriceFrom 
        {
            get
            {
                return _priceFrom;
            }
            set
            {
                if (_priceFrom != value)
                {
                    _priceFrom = value;
                    OnPropertyChanged(nameof(PriceFrom)); 
                }
            }
        }
        private double _priceTo = 0;
        public double PriceTo
        {
            get
            {
                return _priceTo;
            }
            set
            {
                if (_priceTo != value)
                {
                    _priceTo = value; 
                    OnPropertyChanged(nameof(PriceTo));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; 
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
