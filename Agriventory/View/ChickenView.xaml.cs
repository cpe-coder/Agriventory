using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;

namespace Agriventory.View;

public partial class ChickenView : UserControl
{

    public ObservableCollection<ChickenItem> ChickensData { get; set; }
    private readonly MongoDBService _mongoService;
    public ChickenView()
    {
        InitializeComponent();
        _ = LoadChickensDataAsync();
        _mongoService = new MongoDBService();
        ChickensData = new ObservableCollection<ChickenItem>();
        DataContext = new ChickenViewModel();

    }
    private async Task LoadChickensDataAsync()
    {
        var list = await _mongoService.GetAllChickensAsync();
        
        ChickensData.Clear();

        foreach (var item in list)
        {
            ChickensData.Add(item);
            MessageBox.Show(item.ProductName);
        }
        
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedItem = (ChickenItem)((FrameworkElement)button).DataContext;
        MessageBox.Show($"You can now edit {selectedItem.ProductName}");
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedItem = (ChickenItem)((FrameworkElement)button).DataContext;
        MessageBox.Show($"Are you sure you want to delete {selectedItem.ProductName}?");
    }
    
    
    private void AddNewButton_Click(object sender, RoutedEventArgs e)
    {
        AddProductModal.Visibility = Visibility.Visible;
    }

    private void CancelModal_Click(object sender, RoutedEventArgs e)
    {
        AddProductModal.Visibility = Visibility.Collapsed;
    }

    private async void SaveProduct_Click(object sender, RoutedEventArgs e)
    {
        string productName = ProductNameTextBox.Text;
        string stocks = StocksTextBox.Text;
        string brand = BrandTextBox.Text;
        DateTime? dateImported = DateImportedPicker.SelectedDate;

        if (string.IsNullOrWhiteSpace(productName) ||
            string.IsNullOrWhiteSpace(stocks) ||
            string.IsNullOrWhiteSpace(brand) ||
            !dateImported.HasValue)
        {
            MessageBox.Show("Please fill out all fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(stocks, out int stockValue))
        {
            MessageBox.Show("Stocks must be a number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // ✅ Create a ChickenItem object from the modal fields
        var newChicken = new ChickenItem
        {
            ProductName = productName,
            Stocks = stockValue,
            Brand = brand,
            DateImported = dateImported.Value
        };

        try
        {
            await _mongoService.AddChickenAsync(newChicken);
            MessageBox.Show("Product saved successfully!:", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // ✅ Clear fields and close modal
        ProductNameTextBox.Clear();
        StocksTextBox.Clear();
        BrandTextBox.Clear();
        DateImportedPicker.SelectedDate = DateTime.Now;
        AddProductModal.Visibility = Visibility.Collapsed;

        // Optional: reload data if you want to refresh the grid
        await LoadChickensDataAsync();
    }
}
   




