using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;

namespace Agriventory.View;

public partial class TransactionView
{
    private readonly MongoDbService _mongoService;
    public TransactionView()
    {
        InitializeComponent();
        _mongoService = new MongoDbService();
        var viewModel = new TransactionViewModel();
        DataContext = viewModel;
        LoadTransactions();
    }
    private async void LoadTransactions()
    {
        try
        {
            var manila = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var items = await _mongoService.GetAllTransactionsAsync();
            foreach (var item in items)
            {
                item.DateOfDelivery = TimeZoneInfo.ConvertTime(item.DateOfDelivery, manila);
            }
            TransactionDataGrid.ItemsSource = new ObservableCollection<TransactionItem>(items);
            var index = 1;
            foreach (var item in items)
            {
                item.Number = index++;
            }

            TransactionDataGrid.ItemsSource = new ObservableCollection<TransactionItem>(items);
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TransactionGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }
}