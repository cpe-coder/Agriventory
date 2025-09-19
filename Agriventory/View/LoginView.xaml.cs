using System.Windows;
using System.Windows.Input;
using Agriventory.Model;
using MongoDB.Driver;

namespace Agriventory.View;

public partial class LoginView : Window
{

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
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Empty field!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); 
                return;
            }
            
            var userCollections = _mongoService.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq(u => u.Username, username) & Builders<User>.Filter.Eq(u => u.Password, password);
            var user = await userCollections.Find(filter).FirstOrDefaultAsync();

            LoginButton.Content = "Loading...";
            LoginButton.IsEnabled = false;
            
            if (user != null)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                var dashboard = new DashboardView();
                dashboard.Show();
                this.Close();
                return;
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

    private void GetSignInAsCashier_Checked(object sender, RoutedEventArgs e)
    {
        bool? isChecked = myCheckBox.IsChecked;
        
       
    }
}