using System.Collections.Generic;
using System.Net;
using Avalonia.Media;
using BoTech.StarClock.Client.ViewModels.Pages;
using Material.Icons;
using ReactiveUI;


namespace BoTech.StarClock.Client.ViewModels.Dialogs;

public class AddDeviceDialogViewModel : DialogPageBase
{
    private string _ipAddress = "";
    public string IpAddress
    {
        get => _ipAddress;
        set
        {
            _ipAddress = value;
            if(this.Dialog !=  null) this.Dialog.DialogDataModel = value;
            UpdateHelpBox();
        }
    }
    private string _helpString = "";
    public string HelpString
    {
        get => _helpString;
        set => this.RaiseAndSetIfChanged(ref _helpString, value);
    }

    private MaterialIconKind _helpIcon = MaterialIconKind.ButtonPointer;
    public MaterialIconKind HelpIcon
    {
        get => _helpIcon;
        set => this.RaiseAndSetIfChanged(ref _helpIcon, value);
    }

    private IBrush _helpIconColor = Brushes.Blue;
    public IBrush HelpIconColor
    {
        get => _helpIconColor;
        set => this.RaiseAndSetIfChanged(ref _helpIconColor, value);
    }
    private List<DeviceInfoViewModel> _devices;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="devices">All Devices that are already listed in the System.</param>
    public AddDeviceDialogViewModel(List<DeviceInfoViewModel> devices)
    {
        _devices = devices;
    }
    public override void OnPageShow()
    {
        if (DialogSettings != null)
        {
            DialogSettings.Icon = MaterialIconKind.ButtonPointer;
            DialogSettings.IconColor = Brushes.Blue;
        }
    }
    private void UpdateHelpBox()
    {
        if (_devices.Find(d => d.DeviceInfo.IpAddress == IpAddress) != null)
        {
            HelpString = "Device already exists.";
            HelpIcon = MaterialIconKind.Error;
            HelpIconColor = Brushes.Orange;
            DisableDialogButton("Next");
        }
        else
        {
            if (IPAddress.TryParse(IpAddress, out var ipAddress) && ContainsStringThreeDots(IpAddress))
            {
                HelpString = "";
                HelpIcon = MaterialIconKind.Check;
                HelpIconColor = Brushes.Green;
                EnableDialogButton("Next");
            }
            else
            {
                HelpString = "IP not valid";
                HelpIcon = MaterialIconKind.Error;
                HelpIconColor = Brushes.Orange;
                DisableDialogButton("Next");
            }
        }
    }
    
    private bool ContainsStringThreeDots(string s)
    {
        int countedDots = 0;
        foreach (var c in s)
        {
            if(c == '.') countedDots++;
            if(countedDots == 3) return true;
        } 
        return false;
    }
}