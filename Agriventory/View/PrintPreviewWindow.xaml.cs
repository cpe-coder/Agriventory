using System.Windows;
using System.Windows.Documents;

namespace Agriventory.View;

public partial class PrintPreviewWindow : Window
{
    public PrintPreviewWindow(FlowDocument doc)
    {
        InitializeComponent();
        PreviewReader.Document = doc;
    }
}