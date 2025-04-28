using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;

namespace Agriventory.View;

public partial class PigView : UserControl
{
    public PigView()
    {
        InitializeComponent();
        LoadData();
    }
    private void LoadData()
    {
        var items = new List<PigItem>
        {
            new PigItem { Number = 1, Product = "Sample A", Stocks = 100, Category = "Uno feeds", DateImported = DateTime.Now },
            new PigItem { Number = 2, Product = "Sample B", Stocks = 80, Category = "Uhanco", DateImported = DateTime.Now },
            new PigItem { Number = 3, Product = "Sample C", Stocks = 60, Category = "Cargill", DateImported = DateTime.Now.AddDays(-1) },
            new PigItem { Number = 4, Product = "Sample D", Stocks = 150, Category = "Feedpro", DateImported = DateTime.Now.AddDays(-2) }
        };

        FeedsDataGrid.ItemsSource = items;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedItem = (ChickenItem)((FrameworkElement)button).DataContext;
        MessageBox.Show($"You can now edit {selectedItem.Product}");
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedItem = (ChickenItem)((FrameworkElement)button).DataContext;
        MessageBox.Show($"Are you sure you want to delete {selectedItem.Product}?");
    }
}