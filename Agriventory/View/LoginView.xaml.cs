using System.Windows;
using System.Windows.Input;
using Agriventory.Model;
using MongoDB.Driver;

namespace Agriventory.View;

public partial class LoginView : Window
{

    private bool? _isAdmin = false;
    private string _role = "admin";
    
    private readonly MongoDBService _mongoService;
    public LoginView()
    {
        InitializeComponent();
        _mongoService = new MongoDBService();
    }
    
  
    private void LoginView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }


    private void Close_Method(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void LoginButtonClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Empty field!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); 
                return;
            }
            
            var userCollections = _mongoService.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq(u => u.Username, username) & Builders<User>.Filter.Eq(u => u.Password, password) & Builders<User>.Filter.Eq(u => u.Role, _role);
            
            var user = await userCollections.Find(filter).FirstOrDefaultAsync();

            LoginButton.Content = "Loading...";
            LoginButton.IsEnabled = false;
            
            if (user != null)
            {
                if (_isAdmin != null && _isAdmin.Value != false && user.Role != "admin")
                {
                    MessageBox.Show("Cashier login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var dashboard = new CashierDashboardView();
                    dashboard.Show();
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Admin login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var dashboard = new AdminDashboardView();
                    dashboard.Show();
                    this.Close();
                    return;
                }
            }

            MessageBox.Show("Invalid username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            LoginButton.Content = "Login";
            LoginButton.IsEnabled = true;
        }
        catch (Exception err)
        {
            MessageBox.Show($"Error: {err.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            LoginButton.Content = "Login";
            LoginButton.IsEnabled = true;
        }
       
    }

    private void GetSigninCashier_Checked(object sender, RoutedEventArgs em)
    {
        _isAdmin = myCheckBox.IsChecked;
        _role = _isAdmin == true ? "cashier" : "admin";
        var roles = new Roles
        {
            Role = _role
        };

        MessageBox.Show($"Role: {roles.Role}", "", MessageBoxButton.OK, MessageBoxImage.Information);


    }
}