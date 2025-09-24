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
            new TransactionItem { Number = 1, Names = "Sample", Products = "Chicken Feed A", Quantity = "2 Sacks", Category = "Chicken", Brand = "Brand A", DateDelivered = DateTime.Now },
            new TransactionItem { Number = 2, Names = "Sample", Products = "Pig Feed B", Quantity = "2 Sacks", Category = "Pig", Brand = "Brand B", DateDelivered = DateTime.Now },
            new TransactionItem { Number = 3, Names = "Sample", Products = "Chicken Feed B", Quantity = "2 Sacks", Category = "Chicken", Brand = "Brand C", DateDelivered = DateTime.Now.AddDays(-1) },
            new TransactionItem { Number = 4, Names = "Sample", Products = "Pig Feed C", Quantity = "2 Sacks", Category = "Pig", Brand = "Brand D", DateDelivered = DateTime.Now.AddDays(-2) }
        };

        TransactionDataGrid.ItemsSource = transactions;
    }
}