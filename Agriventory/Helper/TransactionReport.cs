using Agriventory.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Agriventory.Helper;

public class TransactionReport : IDocument
{
    public List<TransactionItem> Items { get; set; } = new();
    public byte[]? Logo { get; set; }

    public DocumentMetadata GetMetadata() =>
        new DocumentMetadata { Title = "Transaction Report" };

    public DocumentSettings GetSettings() =>
        new DocumentSettings();

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(40);  
            page.DefaultTextStyle(x => x.FontSize(10)); 

            page.Header().Element(header =>
            {
                header.Row(row =>
                {
                    row.ConstantItem(60).Element(e =>
                    {
                        if (Logo != null)
                        {
                            e.AlignCenter().AlignMiddle().Image(Logo);
                        }
                        else
                        {
                            e.AlignCenter().AlignMiddle().Placeholder();
                        }
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("AGRITECH")
                            .FontSize(22)
                            .Bold()
                            .FontColor("#ff5757")     
                            .AlignCenter();

                        col.Item().Text("Murillo Agrivet Supply Inventory System")
                            .FontSize(12)
                            .AlignCenter();

                        col.Item().PaddingTop(4).Text("Transaction Report")
                            .FontSize(14)
                            .Bold()
                            .FontColor("#ff5757")     
                            .AlignCenter();
                    });
                });
            });

            page.Content().Element(ComposeContent);

            page.Footer().AlignCenter().Text(text =>
            {
                text.Span("Generated • ").SemiBold().FontSize(10);
                text.Span(DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")).FontSize(10);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingTop(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);     // #
                    columns.RelativeColumn();       // Customer
                    columns.RelativeColumn();       // Product
                    columns.ConstantColumn(50);     // Qty
                    columns.RelativeColumn();       // Category
                    columns.RelativeColumn();       // Brand
                    columns.ConstantColumn(90);     // Date
                });

                table.Header(h =>
                {
                    h.Cell().Element(CellStyle).Text("#").Bold();
                    h.Cell().Element(CellStyle).Text("Customer").Bold();
                    h.Cell().Element(CellStyle).Text("Product").Bold();
                    h.Cell().Element(CellStyle).Text("Qty").Bold();
                    h.Cell().Element(CellStyle).Text("Category").Bold();
                    h.Cell().Element(CellStyle).Text("Brand").Bold();
                    h.Cell().Element(CellStyle).Text("Delivery Date").Bold();
                });

                foreach (var t in Items)
                {
                    table.Cell().Element(CellStyle).Text(t.Number.ToString());
                    table.Cell().Element(CellStyle).Text(t.CustomerName ?? "");
                    table.Cell().Element(CellStyle).Text(t.ProductName ?? "");
                    table.Cell().Element(CellStyle).Text(t.Quantity.ToString());
                    table.Cell().Element(CellStyle).Text(t.Category ?? "");
                    table.Cell().Element(CellStyle).Text(t.Brand ?? "");
                    table.Cell().Element(CellStyle).Text(t.DateOfDelivery.ToString("MM/dd/yyyy"));
                }
            });
        });

        static IContainer CellStyle(IContainer c) =>
            c.Padding(5).Border(1).BorderColor(Colors.Grey.Lighten3);
    }
}