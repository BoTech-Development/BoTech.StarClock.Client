using System.Reactive;
using Avalonia.Media;
using BoTech.StarClock.Client.Controls;
using BoTech.StarClock.Client.Models;
using BoTech.StarClock.Client.Views.Pages;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;
/// <summary>
/// All Infos that will be displayed on the front page
/// </summary>
public class DeviceInfoViewModel : ViewModelBase
{
    public DeviceInfo DeviceInfo { get; set; }
    private string _deviceName = "";
    /// <summary>
    /// Name of the device.
    /// </summary>
    public string DeviceName
    {
        get => _deviceName;
        set => this.RaiseAndSetIfChanged(ref _deviceName, value);
    }
    private string _statusInfo = "";
    /// <summary>
    /// Name of the device.
    /// </summary>
    public string StatusInfo
    {
        get => _statusInfo;
        set => this.RaiseAndSetIfChanged(ref _statusInfo, value);
    }
    private IBrush _statusColor = Brushes.Gray;
    /// <summary>
    /// Color of the small circle
    /// Color | meaning <br/>
    /// Gray | NoStatus => Init<br/>
    /// Green | Connected<br/>
    /// Blue | Traffic<br/>
    /// Red | Disconnected<br/>
    /// Orange | Error or Warning => e.g. Connection aborted  
    /// </summary>
    public IBrush StatusColor
    {
        get => _statusColor;
        set => this.RaiseAndSetIfChanged(ref _statusColor, value);
    }
    private bool _isMoreInfoButtonEnabled = true;
    /// <summary>
    /// Determines if the Command <see cref="MoreInfoCommand"/> is active.
    /// </summary>
    public bool IsMoreInfoButtonEnabled
    {
        get =>  _isMoreInfoButtonEnabled; 
        set =>  this.RaiseAndSetIfChanged(ref _isMoreInfoButtonEnabled, value);
    } 
    public ReactiveCommand<Unit, Unit> MoreInfoCommand { get; set; }

    public DeviceInfoViewModel()
    {
        MoreInfoCommand = ReactiveCommand.Create(() =>
        {
            NavigationControl.Push("NavigationControllerYourStarsTab", new DeviceSettingsPageView()
            {
                DataContext = new DeviceSettingsPageViewModel(this)
            });
        });
    }
}