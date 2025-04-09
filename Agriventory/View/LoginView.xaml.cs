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
}