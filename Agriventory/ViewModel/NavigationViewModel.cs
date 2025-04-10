using System.Windows.Input;
using Agriventory.Utils;

namespace Agriventory.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private object _currentView;
    public object CurrentView
    {
        get { return _currentView; }
        set { _currentView = value; OnPropertyChanged(); }
    }
    
    
    public ICommand HomeCommand { get; set; }
    public ICommand StocksCommand { get; set; }
    public ICommand TransactionCommand { get; set; }
    public ICommand HistoryCommand { get; set; }
    public ICommand AboutCommand { get; set; }

    private void Home(object obj) => CurrentView = new HomeViewModel();
    private void Stocks(object obj) => CurrentView = new StocksViewModel();
    private void Transaction(object obj) => CurrentView = new TransactionViewModel();
    private void History(object obj) => CurrentView = new HistoryViewModel();
    private void About(object obj) => CurrentView = new AboutViewModel();

    public NavigationViewModel()
    
    {
        HomeCommand = new Command(Home);
        StocksCommand = new Command(Stocks);
        TransactionCommand = new Command(Transaction);
        HistoryCommand = new Command(History);
        AboutCommand = new Command(About);

        CurrentView = new HomeViewModel();
    }
}