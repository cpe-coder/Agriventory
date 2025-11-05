using System.Collections.ObjectModel;
using System.Windows;
using Agriventory.Model;

namespace Agriventory.ViewModel;
public class ChickenViewModel : BaseViewModel
{
    private readonly MongoDBService _mongoService;
    private ObservableCollection<ChickenItemDisplay> ChickenData { get; }

    private int _totalCount;
    public int TotalCount
    {
        get => _totalCount;
        set {_totalCount = value; OnPropertyChanged(); }
    }

    private string _productName = null!;
    private int _stocks;
    private string _brand = null!;
    private DateTime _dateImported = DateTime.Now;
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
    public ChickenViewModel()
    {
        _mongoService = new MongoDBService();
        new RelayCommand(async void () => await AddProductAsync());
        ChickenData = new ObservableCollection<ChickenItemDisplay>();
        new RelayCommand(async () => await LoadChickenAsync());
        new RelayCommand(async (obj) => await EditChickenAsync(obj));
        new RelayCommand(async (obj) => await DeleteChickenAsync(obj));
        _ = LoadChickenAsync();
    }
    private async Task LoadChickenAsync()
    {
        var list = await _mongoService.GetAllChickensAsync();
        ChickenData.Clear();

        var index = 1;
        foreach (var item in list.Select(x => new ChickenItemDisplay()
                 {
                     Number = index++,
                     ProductName = x.ProductName,
                     Stocks = x.Stocks,
                     Brand = x.Brand,
                     DateImported = x.DateImported,
                 }).ToList()
    )
            
            ChickenData.Add(item);

        TotalCount = ChickenData.Count;
    }
    private class ChickenItemDisplay
    {
        public string? Id { get; set; }
        public int Number { get; set; }
        public string? ProductName { get; set; }
        public int Stocks { get; set; }
        public string? Brand { get; set; }
        public DateTime DateImported { get; set; }
    }
    private async Task AddProductAsync()
    {
        if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Brand) || Stocks <= 0)
        {
            MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        var newChicken = new ChickenItem()
        {
            ProductName = ProductName,
            Stocks = Stocks,
            Brand = Brand,
            DateImported = DateImported
        };
        await _mongoService.AddChickenAsync(newChicken);

        MessageBox.Show("Product saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        ProductName = string.Empty;
        Stocks = 0;
        Brand = string.Empty;
        DateImported = DateTime.Now;
    }
    private async Task EditChickenAsync(object parameter)
    {
        if (parameter is not ChickenItemDisplay selected)
            return;
        var result = MessageBox.Show($"Do you want to edit {selected.ProductName}?", "Edit Product", MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes) return;

        selected.Stocks += 5;

        var updated = new ChickenItem
        {
            Id = selected.Id,
            ProductName = selected.ProductName,
            Stocks = selected.Stocks,
            Brand = selected.Brand,
            DateImported = selected.DateImported
        };

        await _mongoService.UpdateChickenAsync(updated);
        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        await LoadChickenAsync();
    }
    private async Task DeleteChickenAsync(object parameter)
    {
        if (parameter is not ChickenItemDisplay selected)
            return;

        var result = MessageBox.Show($"Are you sure you want to delete {selected.ProductName}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        await _mongoService.DeleteChickenAsync(selected.Id);
        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        await LoadChickenAsync();
    }
}