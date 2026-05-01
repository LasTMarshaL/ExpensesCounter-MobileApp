
using System.ComponentModel;

namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    public class DateAndTimeFilter : INotifyPropertyChanged // This class is responsiable for keeping model of filter by price
    {
        private DateTime _dateFrom; // The earliest date
        public DateTime DateFrom // Property to get acces to the earliest date
        {
            get
            {
                return _dateFrom; // Return the earliest date 
            }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value; // Set the value of the earliest date as got value
                    OnPropertyChanged(nameof(DateFrom)); // Say UI, that property was changed
                }
            }
        }
        private DateTime _dateTo; // The latest date
        public DateTime DateTo // Property to get acces to the latest date
        {
            get
            {
                return _dateTo; // Return the latest date
            } 
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value; // Set the value of the latest date as got value
                    OnPropertyChanged(nameof(DateTo)); // Say UI, that property was changed
                }
            }
        }

        private TimeSpan _timeFrom; // The earliest time
        public TimeSpan TimeFrom // Property to get acces to the earliest time
        {
            get
            {
                return _timeFrom; // Return the earilest time
            }
            set
            {
                if (_timeFrom != value)
                {
                    _timeFrom = value; // Set the value of the earliest time as got value
                    OnPropertyChanged(nameof(TimeFrom)); // Say UI, that property was changed
                }
            }
        }

        private TimeSpan _timeTo; // The latest time
        public TimeSpan TimeTo // Property to get acces to the latest time
        {
            get
            {
                return _timeTo; // Return the latest time
            }
            set
            {
                if (_timeTo != value) // If the latest time is not equal to some value
                {
                    _timeTo = value; // Set the value of the latest time as got value
                    OnPropertyChanged(nameof(TimeTo)); // Say UI, that property was changed
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
