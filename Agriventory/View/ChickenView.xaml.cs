using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;
using MongoDB.Driver;

namespace Agriventory.View;

public partial class ChickenView : UserControl
{

    private readonly MongoDBService _mongoService;
    private readonly ChickenViewModel _viewModel = new();
    private readonly IMongoCollection<ChickenItem> _chickenCollection;
    private ObservableCollection<ChickenItem> _chickens;
    private ChickenItem _selectedProduct;
    public ChickenView()
    {
        InitializeComponent();
        _mongoService = new MongoDBService();
        _viewModel = new ChickenViewModel();
        DataContext = _viewModel;
        
        LoadProducts();
        
    }
    
  
    private async void LoadProducts()
    {
        try
        {
            var items = await _mongoService.GetAllChickensAsync();
            FeedsDataGrid.ItemsSource = new ObservableCollection<ChickenItem>(items);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
   
    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        FeedsDataGrid.Items.Refresh();
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
        
         RefreshButton_Click(sender, e);

        // Optional: reload data if you want to refresh the grid
    }
    
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            _selectedProduct = (sender as Button)?.Tag as ChickenItem;

            if (_selectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }

            // Populate modal
            EditProductName.Text = _selectedProduct.ProductName;
            EditStocks.Text = _selectedProduct.Stocks.ToString();
            EditBrand.Text = _selectedProduct.Brand;
            EditDateImported.SelectedDate = _selectedProduct.DateImported;

            EditProductModal.Visibility = Visibility.Visible;
        }

        private async void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("No product selected.");
                return;
            }

            try
            {
                _selectedProduct.ProductName = EditProductName.Text;
                _selectedProduct.Stocks = int.Parse(EditStocks.Text);
                _selectedProduct.Brand = EditBrand.Text;
                _selectedProduct.DateImported = EditDateImported.SelectedDate ?? DateTime.Now;

                await _mongoService.UpdateChickenAsync(_selectedProduct);
                LoadProducts();

                EditProductModal.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            EditProductModal.Visibility = Visibility.Collapsed;
        }

        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button)?.Tag as ChickenItem;

            if (product == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    await _mongoService.DeleteChickenAsync(product.Id);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}");
                }
            }
        }
    
}





