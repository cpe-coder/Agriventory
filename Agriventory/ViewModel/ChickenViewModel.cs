using System.Windows;
using System.Windows.Input;
using Agriventory.Model;

namespace Agriventory.ViewModel;

public class ChickenViewModel : BaseViewModel
{
    private readonly MongoDBService _mongoService = new();

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

    public ICommand AddChickenCommand { get; }

    public ChickenViewModel()
    {
        AddChickenCommand = new RelayCommand(async void () => await AddProductAsync());
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
        

        // Clear fields after saving
        ProductName = string.Empty;
        Stocks = 0;
        Brand = string.Empty;
        DateImported = DateTime.Now;
    }
}