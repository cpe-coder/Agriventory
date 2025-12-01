using System.Windows.Documents;

namespace Agriventory.View;

public partial class PrintPreviewWindow
{
    public PrintPreviewWindow(FlowDocument doc)
    {
        InitializeComponent();
        PreviewReader.Document = doc;
    }
}