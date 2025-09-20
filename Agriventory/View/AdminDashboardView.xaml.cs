using System.Windows;
using System.Windows.Input;

namespace Agriventory.View;

public partial class AdminDashboardView : Window
{
    public AdminDashboardView()
    {
        InitializeComponent();
    }
    

    private void DashboardView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        const string messageBoxDescription = "Are you sure you want to Logout?";
        const string caption  = "Confirmation";
        const MessageBoxButton button = MessageBoxButton.YesNo;
        const MessageBoxImage icon = MessageBoxImage.Question;
        MessageBoxResult result = MessageBox.Show(messageBoxDescription, caption, button, icon);

        if (result == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
        
    }


    private void Background_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private bool _isMaximized = false;
    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (_isMaximized)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1000;
                this.Height = 600;

                _isMaximized = false;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                _isMaximized = true;
            }
        }
    }

}