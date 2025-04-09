using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("Hello world");
    }
}