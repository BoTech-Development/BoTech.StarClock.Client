using System.Net.Http;
using System.Reactive;
using BoTech.StarClock.Client.Controls;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.Views.Pages;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class DeviceSettingsPageViewModel : ViewModelBase
{
    private string _currentDeviceName = string.Empty;
    public string CurrentDeviceName
    {
        get => _currentDeviceName;
        set => this.RaiseAndSetIfChanged(ref _currentDeviceName, value);
    }
    public ReactiveCommand<Unit, Unit> SaveNewDeviceNameCommand { get; }
    /// <summary>
    /// Shows a page where the user can upload and delete images.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ManageUploadedImagesCommand { get; }
    /// <summary>
    /// Shows a page where the user can add, remove and rearrange slideshow images.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ManageSlideshowCommand { get; }
    private readonly DeviceInfoViewModel _infoViewModel;
    public DeviceSettingsPageViewModel(DeviceInfoViewModel model)
    {
        _infoViewModel = model;
        CurrentDeviceName = _infoViewModel.DeviceName;
        SaveNewDeviceNameCommand = ReactiveCommand.Create(SetNewDeviceName);
        ManageUploadedImagesCommand = ReactiveCommand.Create(ManageUploadedImages);
        ManageSlideshowCommand = ReactiveCommand.Create(ManageSlideshow);
    }
    /// <summary>
    /// Shows a page where the user can upload and delete images.
    /// </summary>
    private void ManageUploadedImages()
    {
        NavigationControl.Push("NavigationControllerYourStarsTab", new ManageUploadedImagesPageView()
        {
            DataContext = new ManageUploadedImagesPageViewModel(_infoViewModel)
        });
    }
    /// <summary>
    /// Shows a page where the user can add, remove and rearrange slideshow images.
    /// </summary>
    private void ManageSlideshow()
    {
        NavigationControl.Push("NavigationControllerYourStarsTab", new ManageSlideshowPageView()
        {
            DataContext = new ManageSlideshowPageViewModel(_infoViewModel)
        });
    }
    private void SetNewDeviceName()
    {
        RequestResult<bool> result = _infoViewModel.DeviceInfo.ApiClient.DeviceInfoClient.SetDeviceName(CurrentDeviceName);
        if (result.ParsedData)
        {
            _infoViewModel.DeviceName = CurrentDeviceName;
        }
        else
        {
            CurrentDeviceName = _infoViewModel.DeviceName;
        }
    }
}