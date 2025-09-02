using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BoTech.StarClock.Client.ViewModels.Pages;

//C:\Dev\Android\openjdk\jdk-17.0.8.101-hotspot
namespace BoTech.StarClock.Client.Views.Pages;

public partial class MyStarsPageView : UserControl
{
    public MyStarsPageView()
    {
        Loaded += OnLoaded;
        InitializeComponent();
        
    }
    /// <summary>
    /// Needed to say the ViewModel, that it can begin reconnecting to known devices
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext != null && DataContext is MyStarsPageViewModel vm)
        {
            vm.ReconnectToKnownDevices();
        }
    }
}