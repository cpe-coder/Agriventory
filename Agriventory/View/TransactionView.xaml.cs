using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Agriventory.Model;
using Agriventory.ViewModel;
using System.Windows.Documents;
using System.Windows.Media;
using Agriventory.Helper;
using QuestPDF.Fluent;

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
    
    private void PrintPreview_Click(object sender, RoutedEventArgs e)
    {
        FlowDocument doc = CreateFlowDocument();
        PrintPreviewWindow preview = new PrintPreviewWindow(doc);
        preview.ShowDialog();
    }
    
    private void ExportPDF_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                FileName = "TransactionReport.pdf"
            };

            if (dialog.ShowDialog() != true)
                return;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            string logoPath = Path.Combine(projectDir, "Assets", "dashboardLogo.png");


            byte[]? logoBytes = null;

            if (File.Exists(logoPath))
                logoBytes = File.ReadAllBytes(logoPath);
            else
                MessageBox.Show($"Logo not found:\n{logoPath}");


            var items = TransactionDataGrid.Items.OfType<TransactionItem>().ToList();

            var report = new TransactionReport
            {
                Items = items,
                Logo = logoBytes
            };

            report.GeneratePdf(dialog.FileName);

            MessageBox.Show("PDF Exported Successfully!", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "PDF Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }





    private FlowDocument CreateFlowDocument()
{
    FlowDocument doc = new FlowDocument();
    doc.PageWidth = 800;
    doc.PagePadding = new Thickness(50);
    doc.FontSize = 14;
    doc.ColumnWidth = 800;

    Paragraph title = new Paragraph(new Run("Transaction Report"))
    {
        FontSize = 24,
        FontWeight = FontWeights.Bold,
        TextAlignment = TextAlignment.Center,
        Margin = new Thickness(0, 0, 0, 20)
    };
    doc.Blocks.Add(title);

    Table table = new Table();
    doc.Blocks.Add(table);

    foreach (var unused in TransactionDataGrid.Columns)
        table.Columns.Add(new TableColumn());

    TableRowGroup headerGroup = new TableRowGroup();
    TableRow header = new TableRow();

    foreach (var col in TransactionDataGrid.Columns)
    {
        header.Cells.Add(new TableCell(new Paragraph(new Run(col.Header.ToString())))
        {
            FontWeight = FontWeights.Bold,
            Padding = new Thickness(5),
            
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.5)
        });
    }

    headerGroup.Rows.Add(header);
    table.RowGroups.Add(headerGroup);

    TableRowGroup dataGroup = new TableRowGroup();

    foreach (var row in TransactionDataGrid.Items)
    {
        if (row is not TransactionItem item) continue;

        TableRow r = new TableRow();
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.Number.ToString()))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.CustomerName))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.ProductName))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.Quantity.ToString()))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.Category))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.Brand))));
        r.Cells.Add(new TableCell(new Paragraph(new Run(item.DateOfDelivery.ToString("MM/dd/yyyy")))));

        foreach (var cell in r.Cells)
        {
            cell.Padding = new Thickness(5);
            cell.BorderBrush = Brushes.Gray;
            cell.BorderThickness = new Thickness(0.3);
        }

        dataGroup.Rows.Add(r);
    }

    table.RowGroups.Add(dataGroup);
    return doc;
}

}