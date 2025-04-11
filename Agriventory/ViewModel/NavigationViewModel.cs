using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Agriventory.Model;

namespace Agriventory.ViewModel;

public class NavigationViewModel : INotifyPropertyChanged
{
    private CollectionViewSource MenuItemsCollection;

    public ICollectionView SourceCollection => MenuItemsCollection.View;

    public NavigationViewModel()
    {
        ObservableCollection<MenuItems> menuItems = new ObservableCollection<MenuItems>
        {
            new MenuItems { MenuName = "Dashboard" , MenuImage = "/Assets/home.png"},
            new MenuItems { MenuName = "Chicken", MenuImage = "/Assets/chicken.png"},
            new MenuItems { MenuName = "Pig", MenuImage = "/Assets/pig.png"},
            new MenuItems { MenuName = "Transaction", MenuImage = "/Assets/transaction.png"},
            new MenuItems { MenuName = "About", MenuImage = "/Assets/user.png"}
        };


        MenuItemsCollection = new CollectionViewSource { Source = menuItems };
        MenuItemsCollection.Filter += MenuItems_Filter;

        SelectedViewModel = new StartupViewModel();

    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    private string filterText;
    public string FilterText
    {
        get => filterText;
        set
        {
            filterText = value;
            MenuItemsCollection.View.Refresh();
            OnPropertyChanged("FilterText");
        }
    }

    private void MenuItems_Filter(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrEmpty(FilterText))
        {
            e.Accepted = true;
            return;
        }

        MenuItems _items = e.Item as MenuItems;
        if (_items.MenuName.ToUpper().Contains(FilterText.ToUpper()))
        {
            e.Accepted = true;
        }
        else
        {
            e.Accepted = false;
        }
    }
    
    private object _selectedViewModel;
    public object SelectedViewModel
    {
        get => _selectedViewModel;
        set { _selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
    }
    
    public void SwitchViews(object parameter)
    {
        switch(parameter)
        {
            case "Home":
                SelectedViewModel = new HomeViewModel();
                break;
            case "Chicken":
                SelectedViewModel = new ChickenViewModel();
                break;
            case "Pig":
                SelectedViewModel = new PigViewModel();
                break;
            case "Transaction":
                SelectedViewModel = new TransactionViewModel();
                break;
            case "About":
                SelectedViewModel = new AboutViewModel();
                break;
            default:
                SelectedViewModel = new HomeViewModel();
                break;
        }
    }
    
    private ICommand _menucommand;
    public ICommand MenuCommand
    {
        get
        {
            if (_menucommand == null)
            {
                _menucommand = new RelayCommand(param => SwitchViews(param));
            }
            return _menucommand;
        }
    }
    

}