using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Agriventory.ViewModel;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly MongoDbService _dbService;
    
    private int _totalStocks;
    private int _totalTransactions;
    private int _totalPigProducts;
    private int _totalChickenProducts;
    private ObservableCollection<BrandStockInfo> _chickenBrands = null!;
    private ObservableCollection<BrandStockInfo> _pigBrands = null!;

    public HomeViewModel()
    {
        _dbService = new MongoDbService();
        ChickenBrands = new ObservableCollection<BrandStockInfo>();
        PigBrands = new ObservableCollection<BrandStockInfo>();
        LoadDashboardData();
    }

    public int TotalStocks
    {
        get => _totalStocks;
        set
        {
            _totalStocks = value;
            OnPropertyChanged();
        }
    }

    public int TotalTransactions
    {
        get => _totalTransactions;
        set
        {
            _totalTransactions = value;
            OnPropertyChanged();
        }
    }

    public int TotalPigProducts
    {
        get => _totalPigProducts;
        set
        {
            _totalPigProducts = value;
            OnPropertyChanged();
        }
    }

    public int TotalChickenProducts
    {
        get => _totalChickenProducts;
        set
        {
            _totalChickenProducts = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<BrandStockInfo> ChickenBrands
    {
        get => _chickenBrands;
        set
        {
            _chickenBrands = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<BrandStockInfo> PigBrands
    {
        get => _pigBrands;
        set
        {
            _pigBrands = value;
            OnPropertyChanged();
        }
    }

    private async void LoadDashboardData()
    {
        try
        {
            TotalStocks = await _dbService.GetTotalStocksAsync();
            TotalTransactions = await _dbService.GetTotalTransactionsAsync();
            TotalPigProducts = await _dbService.GetTotalPigProductsAsync();
            TotalChickenProducts = await _dbService.GetTotalChickenProductsAsync();

            ChickenBrands.Clear();
            var chickenBrandStocks = await _dbService.GetChickenBrandsWithStocksAsync();
            
            foreach (var brand in chickenBrandStocks)
            {
                ChickenBrands.Add(new BrandStockInfo
                {
                    BrandName = brand.Key,
                    TotalStocks = brand.Value
                });
            }

            PigBrands.Clear();
            var pigBrandStocks = await _dbService.GetPigBrandsWithStocksAsync();
            
            foreach (var brand in pigBrandStocks)
            {
                PigBrands.Add(new BrandStockInfo
                {
                    BrandName = brand.Key,
                    TotalStocks = brand.Value
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class BrandStockInfo
{
    public string BrandName { get; set; } = null!;
    public int TotalStocks { get; set; }
}