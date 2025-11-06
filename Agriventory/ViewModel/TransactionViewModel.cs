using System.Collections.ObjectModel;
using Agriventory.Model;

namespace Agriventory.ViewModel;

public class TransactionViewModel : BaseViewModel
{
    
    private readonly MongoDbService _mongoService;
    private ObservableCollection<DeliveryPigItemDisplay> PigData { get; }
    public TransactionViewModel()
    {
        _mongoService = new MongoDbService();
        PigData = new ObservableCollection<DeliveryPigItemDisplay>();
        new RelayCommand(async () => await LoadPigAsync());
        _ = LoadPigAsync();
    }
    private async Task LoadPigAsync()
    {
        var list = await _mongoService.GetAllTransactionsAsync();
        PigData.Clear();

        var index = 1;
        foreach (var item in list.Select(x => new DeliveryPigItemDisplay()
                 {
                     Number = index++,
                     CustomerName = x.CustomerName,
                     ProductName = x.ProductName,
                     Brand = x.Brand,
                     Quantity = x.Quantity,
                     Category = x.Category,
                     DateOfDelivery = x.DateOfDelivery,
                 }).ToList()
                )
            
            PigData.Add(item);

        TotalCount = PigData.Count;
    }

    public int TotalCount { get; set; }

    private class DeliveryPigItemDisplay
    {
        public string? Id { get; set; }
        public int Number { get; set; }
        public string? CustomerName { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public DateTime DateOfDelivery { get; set; }
    }
    
}