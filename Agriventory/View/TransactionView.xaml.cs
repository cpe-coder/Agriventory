using System.Windows.Controls;
using Agriventory.Model;

namespace Agriventory.View;

public partial class TransactionView : UserControl
{
    public TransactionView()
    {
        InitializeComponent();
        LoadTransactions();
    }
    private void LoadTransactions()
    {
        var transactions = new List<TransactionItem>
        {
            new TransactionItem { Number = 1, Product = "Chicken Feed A", Stocks = 100, Category = "Chicken", Brand = "Brand A", DateDelivered = DateTime.Now },
            new TransactionItem { Number = 2, Product = "Pig Feed B", Stocks = 80, Category = "Pig", Brand = "Brand B", DateDelivered = DateTime.Now },
            new TransactionItem { Number = 3, Product = "Chicken Feed B", Stocks = 50, Category = "Chicken", Brand = "Brand C", DateDelivered = DateTime.Now.AddDays(-1) },
            new TransactionItem { Number = 4, Product = "Pig Feed C", Stocks = 70, Category = "Pig", Brand = "Brand D", DateDelivered = DateTime.Now.AddDays(-2) }
        };

        TransactionDataGrid.ItemsSource = transactions;
    }
}