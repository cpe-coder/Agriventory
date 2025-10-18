using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;

namespace Agriventory.View;

public partial class ChickenView : UserControl
{
    public ChickenView()
    {
        InitializeComponent();
        LoadData();
    }
    private void LoadData()
    {
        var items = new List<ChickenItem>
        {
            
            
        };

        FeedsDataGrid.ItemsSource = items;
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

    private void SaveProduct_Click(object sender, RoutedEventArgs e)
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

        // TODO: Add logic to insert to database or collection
        MessageBox.Show($"Product '{productName}' added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        // Clear fields and close modal
        ProductNameTextBox.Clear();
        StocksTextBox.Clear();
        BrandTextBox.Clear();
        DateImportedPicker.SelectedDate = DateTime.Now;
        AddProductModal.Visibility = Visibility.Collapsed;
    }
}
   




