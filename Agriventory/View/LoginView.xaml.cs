using System.Windows;
using System.Windows.Input;
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


    private async void LoginButtonClicked(object sender, RoutedEventArgs e)
    {
        string username = Username.Text.Trim();
        string password = Password.Password;

        ErrorMessage.Text = "";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorMessage.Text = "Empty fields.";
            return;
        }

        LoginButton.Content = "Loading...";


        await Task.Delay(2000);

        if (username != "admin" || password != "admin")
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
        }

        


    }
}