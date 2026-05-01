using System.ComponentModel;

namespace ExpensesCounterMobileApplication;

public class ExpensesCategoryViewModel: INotifyPropertyChanged // This class is responsiable for keeping model of expenses category.
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