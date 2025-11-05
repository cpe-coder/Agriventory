using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;

namespace Agriventory.View;

public partial class ChickenView
{

    private readonly MongoDBService _mongoService;
    private ChickenItem? _selectedProduct;
    public ChickenView()
    {
        InitializeComponent();
        _mongoService = new MongoDBService();
        var viewModel = new ChickenViewModel();
        DataContext = viewModel;
        
        LoadProducts();
    }
    private async void LoadProducts()
    {
        try
        {
            var manila = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var items = await _mongoService.GetAllChickensAsync();
            foreach (var item in items)
            {
                item.DateImported = TimeZoneInfo.ConvertTime(item.DateImported, manila);
            }
            FeedsDataGrid.ItemsSource = new ObservableCollection<ChickenItem>(items);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
        var productName = ProductNameTextBox.Text;
        var stocks = StocksTextBox.Text;
        var brand = BrandTextBox.Text;
        var dateImported = DateImportedPicker.SelectedDate;

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
        
        var manilaTz = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        DateTime manilaDateTime = TimeZoneInfo.ConvertTime(
            dateImported.Value.Date.Add(DateTime.Now.TimeOfDay),
            manilaTz
        );

        var newChicken = new ChickenItem
        {
            ProductName = productName,
            Stocks = stockValue,
            Brand = brand,
            DateImported = manilaDateTime,
            DateUpdated = manilaDateTime
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
        ProductNameTextBox.Clear();
        StocksTextBox.Clear();
        BrandTextBox.Clear();
        DateImportedPicker.SelectedDate = null;
        AddProductModal.Visibility = Visibility.Collapsed;
        
        LoadProducts();

    }
    
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            _selectedProduct = (sender as Button)?.Tag as ChickenItem;

            if (_selectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }
            EditProductName.Text = _selectedProduct.ProductName!;
            EditStocks.Text = _selectedProduct.Stocks.ToString();
            EditBrand.Text = _selectedProduct.Brand!;
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
                var manilaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                var selectedDate = EditDateImported.SelectedDate ?? DateTime.Now;
                var currentManilaTime = TimeZoneInfo.ConvertTime(DateTime.Now, manilaTimeZone).TimeOfDay;
                var combinedDateTime = new DateTime(
                    selectedDate.Year,
                    selectedDate.Month,
                    selectedDate.Day,
                    currentManilaTime.Hours,
                    currentManilaTime.Minutes,
                    currentManilaTime.Seconds,
                    DateTimeKind.Unspecified
                );
                _selectedProduct.ProductName = EditProductName.Text;
                _selectedProduct.Stocks = int.Parse(EditStocks.Text);
                _selectedProduct.Brand = EditBrand.Text;
                _selectedProduct.DateImported = TimeZoneInfo.ConvertTime( combinedDateTime, manilaTimeZone);

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

     
        private void CancelDeliver_Click(object sender, RoutedEventArgs e)
        {
            DeliveryProductModal.Visibility = Visibility.Hidden;
        }
        private async void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            DeliveryProductModal.Visibility = Visibility.Visible;
            try
            {
                // Load distinct product names for dropdown
                var chickens = await _mongoService.GetAllChickensAsync();
                ProductNameComboBox.ItemsSource = chickens
                    .Select(c => c.ProductName)
                    .Distinct()
                    .ToList();

                ProductNameComboBox.SelectedIndex = -1;
                BrandComboBox.ItemsSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ProductNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductNameComboBox.SelectedItem is not string selectedProduct) return;
            var chickens = await _mongoService.GetAllChickensAsync();
            BrandComboBox.ItemsSource = chickens
                .Where(c => c.ProductName == selectedProduct)
                .Select(c => c.Brand)
                .Distinct()
                .ToList();
        }
        
           private async void SaveDelivery_Click(object sender, RoutedEventArgs e)
        {
            var customerName = CustomersName.Text;
            var productName = ProductNameComboBox.Text;
            var quantity = QuantityTextBox.Text;
            var brand = BrandComboBox.Text;
            var dateDelivery = DateDelivery.SelectedDate;
            
            if (string.IsNullOrWhiteSpace(customerName) ||
                string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(quantity) ||
                string.IsNullOrWhiteSpace(brand) ||
                !dateDelivery.HasValue)
            {
                MessageBox.Show($"Please fill out all fields.{customerName}, {productName}, {quantity}, {brand}, {dateDelivery}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(quantity, out int quantityValue))
            {
                MessageBox.Show("Quantity must be a number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newDelivery = new DeliveryChickenItem
            {
                CustomerName = customerName,
                ProductName = productName,
                Quantity = quantityValue,
                Brand = brand,
                DateDelivery = DateTime.Now
            };

            try
            {
                await _mongoService.DeliveryChickenAsync(newDelivery);
                MessageBox.Show("Delivery saved successfully!:", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving delivery: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }  
            CustomersName.Clear();
            QuantityTextBox.Clear();
            DateDelivery.SelectedDate = DateTime.Now;
            DeliveryProductModal.Visibility = Visibility.Collapsed;
        
            LoadProducts();
            
        }
        
     
        
}





