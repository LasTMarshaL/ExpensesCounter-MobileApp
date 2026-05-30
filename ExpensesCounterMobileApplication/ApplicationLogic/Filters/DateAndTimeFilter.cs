
using System.ComponentModel;

namespace ExpensesCounterMobileApplication
{
    public class DateAndTimeFilter : INotifyPropertyChanged
    {
        private DateTime _dateFrom; 
        public DateTime DateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                }
            }
        }
        private DateTime _dateTo;
        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            } 
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    OnPropertyChanged(nameof(DateTo));
                }
            }
        }

        private TimeSpan _timeFrom;
        public TimeSpan TimeFrom
        {
            get
            {
                return _timeFrom;
            }
            set
            {
                if (_timeFrom != value)
                {
                    _timeFrom = value;
                    OnPropertyChanged(nameof(TimeFrom));
                }
            }
        }

        private TimeSpan _timeTo; 
        public TimeSpan TimeTo 
        {
            get
            {
                return _timeTo; 
            }
            set
            {
                if (_timeTo != value) 
                {
                    _timeTo = value;
                    OnPropertyChanged(nameof(TimeTo));
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
