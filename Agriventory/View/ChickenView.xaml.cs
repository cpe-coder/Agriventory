using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;

namespace Agriventory.View;

public partial class ChickenView : UserControl
{

    private readonly MongoDBService _mongoService;
    private readonly ChickenViewModel _viewModel;
    public ChickenView()
    {
        InitializeComponent();
        _mongoService = new MongoDBService();
        _viewModel = new ChickenViewModel();
        DataContext = _viewModel;

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
    }
}
   




