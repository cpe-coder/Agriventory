using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Agriventory.Model;
using Agriventory.Utils;

namespace Agriventory.ViewModel;

public class NavigationViewModel : INotifyPropertyChanged
{
    private CollectionViewSource MenuItemsCollection;

    public ICollectionView SourceCollection => MenuItemsCollection.View;

    public NavigationViewModel()
    {
        ObservableCollection<MenuItems> menuItems = new ObservableCollection<MenuItems>
        {
            new() { MenuName = "Home" },
            new() { MenuName = "Chicken" },
            new() { MenuName = "Pig" },
            new() { MenuName = "Transaction" },
            new() { MenuName = "About" }
        };


        MenuItemsCollection = new CollectionViewSource { Source = menuItems };
        MenuItemsCollection.Filter += MenuItems_Filter;

        SelectedViewModel = new StartupViewModel();

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    private string _filterText;
    public string FilterText
    {
        get => _filterText;
        set
        {
            _filterText = value;
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

        MenuItems? items = e.Item as MenuItems;
        if (items == null) throw new ArgumentNullException(nameof(items));
        if (items.MenuName.ToUpper().Contains(FilterText.ToUpper()))
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

    private void SwitchViews(object parameter)
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