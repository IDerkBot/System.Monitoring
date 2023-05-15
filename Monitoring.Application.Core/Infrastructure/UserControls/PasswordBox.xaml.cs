using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoringNetCore.Infrastructure.UserControls;

public partial class PasswordBox : UserControl
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password), typeof(string), typeof(PasswordBox), new PropertyMetadata(default(string)));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
    
    public PasswordBox()
    {
        InitializeComponent();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e) => Password = PbPassword.Password;
}