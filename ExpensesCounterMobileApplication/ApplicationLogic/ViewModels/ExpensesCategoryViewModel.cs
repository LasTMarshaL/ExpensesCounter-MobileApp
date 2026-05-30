using System.ComponentModel;

namespace ExpensesCounterMobileApplication.ApplicationLogic.ViewModels;

public class ExpensesCategoryViewModel: INotifyPropertyChanged
{
    public string? Name { get; set; } 
    public string? Icon { get; set; }

    private double _totalSum; 
    public double TotalSum 
    {
        get
        {
            return _totalSum;
        }
        set
        {
            if (_totalSum != value) 
            {
                _totalSum = value;
                OnPropertyChanged(nameof(TotalSum)); 
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged; 

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}