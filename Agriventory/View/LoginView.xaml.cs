using System.Windows;
using System.Windows.Input;
using MongoDB.Driver;

namespace Agriventory.View;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
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

    private readonly MongoDBService _mongoService = new MongoDBService();
    private async void LoginButtonClicked(object sender, RoutedEventArgs e)
    {
        string username = Username.Text.Trim();
        string password = Password.Password;

        ErrorMessage.Text = "";
        
        var usersCollection = _mongoService.GetUsersCollection();
        var filter = Builders<User>.Filter.Eq(u => u.Username, username) & Builders<User>.Filter.Eq(u => u.Password, password);
        var user = await usersCollection.Find(filter).FirstOrDefaultAsync();
        
        Console.WriteLine(user);

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorMessage.Text = "Empty fields.";
            return;
        }

        LoginButton.Content = "Loading...";


        await Task.Delay(2000);

        if (username != user.Username || password != user.Password)
        {
            ErrorMessage.Text = "Invalid username or password.";
            LoginButton.Content = "Login";
            LoginButton.IsEnabled = true;
        }
        else
        {
            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information); 
            LoginButton.Content = "Login"; 
            LoginButton.IsEnabled = true;
            var dashboard = new MainWindow();
            dashboard.Show();
            this.Close();
        }

        


    }
}