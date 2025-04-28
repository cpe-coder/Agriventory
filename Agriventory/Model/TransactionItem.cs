namespace Agriventory.Model;

public class TransactionItem
{
    public int Number { get; set; }
    public string Product { get; set; }
    public int Stocks { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public DateTime DateDelivered { get; set; }
}