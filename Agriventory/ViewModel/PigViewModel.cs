using System.Collections.ObjectModel;
using System.Windows;
using Agriventory.Model;

namespace Agriventory.ViewModel;
public class PigViewModel : BaseViewModel
{
    private readonly MongoDbService _mongoService;
    private ObservableCollection<PigItemDisplay> PigData { get; }

    private int _totalCount;
    public int TotalCount
    {
        get => _totalCount;
        set {_totalCount = value; OnPropertyChanged(); }
    }

    private string _customerName = null!;
    private string _productName = null!;
    private int _stocks;
    private int _quantity;
    private string _brand = null!;
    private DateTime _dateImported = DateTime.Now;
    private DateTime _dateUpdated = DateTime.Now;
    private DateTime _dateDelivery = DateTime.Now;

    private string CustomerName
    {
        get => _customerName;
        set { _productName = value; OnPropertyChanged(); }
    }

    private string ProductName
    {
        get => _productName;
        set { _productName = value; OnPropertyChanged(); }
    }
    private int Stocks
    {
        get => _stocks;
        set { _stocks = value; OnPropertyChanged(); }
    }
    private int Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(); }
    }
    private string Brand
    {
        get => _brand;
        set { _brand = value; OnPropertyChanged(); }
    }
    private DateTime DateImported
    {
        get => _dateImported;
        set { _dateImported = value; OnPropertyChanged(); }
    }
    private DateTime DateUpdated
    {
        get => _dateUpdated;
        set { _dateUpdated = value; OnPropertyChanged(); }
    }
    
    private DateTime DateDelivery
    {
        get => _dateDelivery;
        set { _dateDelivery = value; OnPropertyChanged(); }
    }   
    public PigViewModel()
    {
        _mongoService = new MongoDbService();
        new RelayCommand(async void () => await AddProductAsync());
        new RelayCommand(async void () => await DeliveryProductAsync());
        PigData = new ObservableCollection<PigItemDisplay>();
        new RelayCommand(async () => await LoadPigAsync());
        new RelayCommand(async (obj) => await EditPigAsync(obj));
        new RelayCommand(async (obj) => await DeletePigAsync(obj));
        _ = LoadPigAsync();
    }
    private async Task LoadPigAsync()
    {
        var list = await _mongoService.GetAllPigsAsync();
        PigData.Clear();

        var index = 1;
        foreach (var item in list.Select(x => new PigItemDisplay()
                 {
                     Number = index++,
                     ProductName = x.ProductName,
                     Stocks = x.Stocks,
                     Brand = x.Brand,
                     DateImported = x.DateImported,
                     DateUpdated = x.DateUpdated,
                 }).ToList()
    )
            
            PigData.Add(item);

        TotalCount = PigData.Count;
    }
    private class PigItemDisplay
    {
        public string? Id { get; set; }
        public int Number { get; set; }
        public string? ProductName { get; set; }
        public int Stocks { get; set; }
        public string? Brand { get; set; }
        public DateTime DateImported { get; set; }
        public DateTime DateUpdated { get; set; }
    }
    private async Task AddProductAsync()
    {
        if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Brand) || Stocks <= 0)
        {
            MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var newPig = new PigItem()
        {
            ProductName = ProductName,
            Stocks = Stocks,
            Brand = Brand,
            DateImported = DateTime.Now,
            DateUpdated = DateImported,
        };
        await _mongoService.AddPigAsync(newPig);

        MessageBox.Show("Product saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        ProductName = string.Empty;
        Stocks = 0;
        Brand = string.Empty;
        DateImported = TimeZoneInfo.ConvertTime(DateTime.Now,
            TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
        DateUpdated = DateImported;
    }
    private async Task EditPigAsync(object parameter)
    {
        if (parameter is not PigItemDisplay selected)
            return;
        var result = MessageBox.Show($"Do you want to edit {selected.ProductName}?", "Edit Product", MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes) return;

        selected.Stocks += 5;

        var manilaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        var manilaDate = TimeZoneInfo.ConvertTime(selected.DateUpdated, manilaTimeZone);
        var updated = new PigItem
        {
            Id = selected.Id,
            ProductName = selected.ProductName,
            Stocks = selected.Stocks,
            Brand = selected.Brand,
            DateUpdated = manilaDate,
        };

        await _mongoService.UpdatePigAsync(updated);
        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        await LoadPigAsync();
    }
    private async Task DeletePigAsync(object parameter)
    {
        if (parameter is not PigItemDisplay selected)
            return;

        var result = MessageBox.Show($"Are you sure you want to delete {selected.ProductName}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        await _mongoService.DeletePigAsync(selected.Id);
        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        await LoadPigAsync();
    }
    
    
    private async Task DeliveryProductAsync()
    {
        if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Brand) || Stocks <= 0)
        {
            MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var newDelivery = new DeliveryPigItem()
        {
            CustomerName = CustomerName,
            ProductName = ProductName,
            Quantity = Quantity,
            Brand = Brand,
            Category = "Pig",
            DateOfDelivery = DateTime.Now
        };
        await _mongoService.DeliveryPigAsync(newDelivery);

        MessageBox.Show("Product saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        CustomerName = string.Empty;
        ProductName = string.Empty;
        Quantity = 0;
        Brand = string.Empty;
        DateDelivery = DateTime.Now;
    }
}