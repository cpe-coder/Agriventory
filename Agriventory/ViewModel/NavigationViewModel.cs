using System.Windows.Input;
using Agriventory.Utils;

namespace Agriventory.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set { _currentView = value; OnPropertyChanged(); }
    }

    
    public ICommand HomeCommand { get;}
    public ICommand StocksCommand { get;  }
    public ICommand TransactionCommand { get; }
    public ICommand HistoryCommand { get; }
    public ICommand AboutCommand { get;  }

    private void Home() => CurrentView = new HomeViewModel();
    private void Stocks() => CurrentView = new StocksViewModel();
    private void Transaction() => CurrentView = new TransactionViewModel();
    private void History() => CurrentView = new HistoryViewModel();
    private void About() => CurrentView = new AboutViewModel();

    public NavigationViewModel()
        {
            HomeCommand = new RelayCommand(Home);
            StocksCommand = new RelayCommand(Stocks);
            TransactionCommand = new RelayCommand(Transaction);
            HistoryCommand = new RelayCommand(History);
            AboutCommand = new RelayCommand(About);

            CurrentView = new HomeViewModel(); 
        }
}