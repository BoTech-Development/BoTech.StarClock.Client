using System.Net.Http;
using System.Reactive;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class DeviceSettingsPageViewModel : ViewModelBase
{
    private string _currentDeviceName;
    public string CurrentDeviceName
    {
        get => _currentDeviceName;
        set => this.RaiseAndSetIfChanged(ref _currentDeviceName, value);
    }
    public ReactiveCommand<Unit, Unit> SaveNewDeviceNameCommand { get; }
    private DeviceInfoViewModel _infoViewModel;
    public DeviceSettingsPageViewModel(DeviceInfoViewModel model)
    {
        _infoViewModel = model;
        CurrentDeviceName = _infoViewModel.DeviceName;
        SaveNewDeviceNameCommand = ReactiveCommand.Create(SetNewDeviceName);
    }

    private void SetNewDeviceName()
    {
        (bool success, HttpResponseMessage? message) = _infoViewModel.DeviceInfo.ApiClient.SetDeviceName(CurrentDeviceName);
        if (success)
        {
            _infoViewModel.DeviceName = CurrentDeviceName;
        }
        else
        {
            CurrentDeviceName = _infoViewModel.DeviceName;
        }
    }
}