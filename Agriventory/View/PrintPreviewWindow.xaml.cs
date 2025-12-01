using System.Printing;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace Agriventory.View
{
    public partial class PrintPreviewWindow : Window
    {
        private FlowDocument _document;

        public PrintPreviewWindow(FlowDocument doc)
        {
            InitializeComponent();
            _document = doc;

            // Force A4 size in preview
            SetA4PageSize(_document);

            PreviewReader.Document = _document;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                SetA4PageSize(_document);
                _document.ColumnWidth = printDialog.PrintableAreaWidth;

                printDialog.PrintDocument(((IDocumentPaginatorSource)_document).DocumentPaginator,
                    "Transaction Report");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SetA4PageSize(FlowDocument doc)
        {
            // A4 size in device-independent units (1/96 inch)
            // A4 = 8.27 inch × 11.69 inch
            doc.PageWidth = 793;   // 8.27 × 96
            doc.PageHeight = 1122; // 11.69 × 96
            doc.ColumnWidth = doc.PageWidth; 
            doc.PagePadding = new Thickness(50); 
        }
    }
}