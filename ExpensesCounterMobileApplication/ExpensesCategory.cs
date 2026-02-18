
using System.ComponentModel;

namespace ExpensesCounterMobileApplication; // Conect class to the main project namespace

public class ExpensesCategory: INotifyPropertyChanged // This class is responsiable for keeping model of expenses category
{
    public string? Name { get; set; } // Name of the category 
    public string? Icon { get; set; } // Icon of the category

    private double _totalSum; // Total sum of the category
    public double TotalSum // Total sum of the category (Property)
    {
        get
        {
            return _totalSum;
        }
        set
        {
            if (_totalSum != value) // If current value "totalSum" does not equal to value, which suggested to be setted
            {
                _totalSum = value;
                OnPropertyChanged(nameof(TotalSum)); // Send information to UI (XAML page), that property "TotalSum" was changed
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged; // Event, which is executed after changing of property

    protected void OnPropertyChanged(string propertyName) // This method sends information to UI (XAML page), that property was changed
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Send information to UI (XAML page), that property was changed
    }
}