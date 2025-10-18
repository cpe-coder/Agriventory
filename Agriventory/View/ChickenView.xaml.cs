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
}
   




