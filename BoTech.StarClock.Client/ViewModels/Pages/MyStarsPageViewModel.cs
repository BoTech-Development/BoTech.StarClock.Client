using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BoTech.StarClock.Api.SharedModels;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.Helper.ApiClient;
using BoTech.StarClock.Client.Models;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views.Dialog;
using BoTech.StarClock.Client.Views.Dialogs;
using CherylUI.Controls;
using Material.Icons;
using Newtonsoft.Json;
using ReactiveUI;
using Tmds.DBus;
using DeviceInfo = BoTech.StarClock.Client.Models.DeviceInfo;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class MyStarsPageViewModel : ViewModelBase
{
    public ObservableCollection<DeviceInfoViewModel> Devices { get; set; } = new ObservableCollection<DeviceInfoViewModel>();
    public ReactiveCommand<Unit, Unit> AddNewDeviceCommand { get; set; }
    /// <summary>
    /// All devices that are or were connected to this client
    /// </summary>
    private List<DeviceInfo> _knownDevices = new List<DeviceInfo>();
    public MyStarsPageViewModel()
    {
        AddNewDeviceCommand = ReactiveCommand.Create(AddNewDevice);
        TryToLoadSavedChanges();
    }

    /// <summary>
    /// Shows Progress bar and execute <see cref="ReconnectToKnownDeviceAction"/> Action. <br/>
    /// Will be called when the view has been initialized.
    /// </summary>
    public void ReconnectToKnownDevices()
    {
        Devices.Clear();
        ProgressDialogViewModel progressVm = new ProgressDialogViewModel();
        progressVm.Action = ReconnectToKnownDeviceAction;
        ProgressDialogView progressDialogView = new ProgressDialogView()
        {
            DataContext = progressVm
        };
        GenericDialogView dialogView = new GenericDialogView()
        {
            DataContext = new GenericDialogViewModel()
            {
                ContentOfTheDialog = progressDialogView,
                DialogButtons = new List<DialogButton>()
                {
                    new DialogButton()
                    {
                        ButtonText = "Ok",
                        IsEnabled = false,
                        OnClickCommand = ReactiveCommand.Create(() => InteractiveContainer.CloseDialog())
                    }
                }
            }
        };
        InteractiveContainer.ShowDialog(dialogView);
        progressVm.DialogSettings = dialogView.DataContext as GenericDialogViewModel;
        progressVm.OnPageShow();
    }
    /// <summary>
    /// Creates a new connection to each device and loads all infos from the star.<br/>
    /// The Action will be executed when the Progressbar pops up.
    /// </summary>
    private void ReconnectToKnownDeviceAction(object data, ProgressDialogViewModel vm)
    {
        vm.DialogSettings.Icon = MaterialIconKind.Loading;
        vm.DialogSettings.IconColor = Brushes.Blue;
        vm.IsIndeterminate = false;
        vm.Maximum = _knownDevices.Count;
        int index = 0;
        foreach (DeviceInfo device in _knownDevices)
        {
            index++;
            vm.ProgressText = "Try to connect to: " + device.DeviceName;
            (ApiClient? client, HttpResponseMessage? errorMessage) = ApiClient.CreateApiClientFor("http://" + device.IpAddress);
            if (client != null)
            {
                device.ApiClient = client;
                RequestResult<string> result = device.ApiClient.DeviceInfoClient.GetDeviceName();
                if (result.Success && result.ParsedData != null)
                {
                    device.DeviceName = result.ParsedData;
                    Devices.Add(new DeviceInfoViewModel()
                    {
                        DeviceInfo = device,
                        DeviceName = device.DeviceName,
                        StatusColor = Brushes.Green,
                        StatusInfo = "Connected."
                    });
                }else // Can not get the Device Name
                {
                    Devices.Add(new DeviceInfoViewModel()
                    {
                        DeviceInfo = device,
                        DeviceName = "???",
                        StatusColor = Brushes.Orange,
                        StatusInfo = "Can not get Device Name."
                    });
                }
            }
            else // Show Error Message:
            {
                Devices.Add(new DeviceInfoViewModel()
                {
                    DeviceInfo = device,
                    DeviceName = device.DeviceName,
                    StatusColor = Brushes.Red,
                    StatusInfo = "Not connected."
                });
            }
            vm.Value = index;
        }
        vm.DialogSettings.Icon = MaterialIconKind.Check;
        vm.DialogSettings.IconColor = Brushes.Green;
        vm.ProgressText = "Finished.";
        vm.Value = 100;
        DialogButton? button = vm.DialogSettings.DialogButtons.Find(b => b.ButtonText == "Ok");
        if (button != null)
            button.IsEnabled = true;
    }
    /// <summary>
    /// Adds a new device to the list <see cref="Devices"/> when the connection was successfully
    /// </summary>
    private void AddNewDevice()
    {
        AddDeviceDialogViewModel addDeviceVm = new AddDeviceDialogViewModel(Devices.ToList());
        AddDeviceDialogView addDeviceDialogView = new AddDeviceDialogView()
        {
            DataContext = addDeviceVm
        };
        ProgressDialogViewModel progressVm = new ProgressDialogViewModel();
        progressVm.Action = AddDevice;
        ProgressDialogView progressDialogView = new ProgressDialogView()
        {
            DataContext = progressVm
        };
        PagedDialog pagedDialog = new PagedDialog(
            new List<UserControl>()
            {
                addDeviceDialogView,
                progressDialogView,
            });
        pagedDialog.ShowPage(0);
        InteractiveContainer.ShowDialog(pagedDialog);
    }
    /// <summary>
    /// This Method will be called when the user enters the ip address in the AddNewDevice Dialog
    /// </summary>
    /// <param name="data"></param>
    /// <param name="vm"></param>
    /// <returns></returns>
    private void AddDevice(object data, ProgressDialogViewModel vm)
    {
        vm.DialogSettings.Icon = MaterialIconKind.Loading;
        vm.DialogSettings.IconColor = Brushes.Blue;
        vm.IsIndeterminate = true;
        vm.Maximum = 100;
        vm.ProgressText = "Try to connect to: " + data?.ToString();
        try
        {
            // Cleanup the Url
            // ^ => means inverted, synonym to !; 0-9 all Numbers; . and dot
            string plainIp = Regex.Replace( (data as string), "[^0-9.]", "");
            string url = "http://" + plainIp;
            (ApiClient? client, HttpResponseMessage? message) result = ApiClient.CreateApiClientFor(url);
            if (result.client != null)
            {
                GetDeviceNameAndAdd(plainIp, result.client, vm);
            }
            SaveChanges();
        }
        catch (Exception e)
        {
            vm.ProgressText = $"Error: {e} .";
            vm.Value = 100;
            vm.DialogSettings.Icon = MaterialIconKind.Error;
            vm.DialogSettings.IconColor = Brushes.Orange;
            
            Console.WriteLine(e);
            return;
        }
        vm.DialogSettings.Icon = MaterialIconKind.Check;
        vm.DialogSettings.IconColor = Brushes.Green;
        vm.ProgressText = "Finished.";
        vm.Value = 100;
    }
    /// <summary>
    /// Gets the name of the device if it is unknown or has already been connected to the app.<br/>
    /// This Method also adds the new DeviceInfoViewModel to the Devices List
    /// </summary>
    /// <param name="ipAddress">Ip for saving it to the <see cref="Api.SharedModels.DeviceInfo"/> Model</param>
    /// <param name="client">Api Client to invoke the specific endpoints</param>
    /// <param name="vm">View Model for the progressbar</param>
    private void GetDeviceNameAndAdd(string ipAddress, ApiClient client, ProgressDialogViewModel vm)
    {
        DeviceInfo info;
        vm.ProgressText = "Try to get device name from:  " + ipAddress; 
        RequestResult<string> requestResult = client.DeviceInfoClient.GetDeviceName();
        if (requestResult.Success && requestResult.ParsedData != null)
        {
            info = new DeviceInfo(ipAddress, requestResult.ParsedData)
            {
                ApiClient = client
            };
            _knownDevices.Add(info);
            Devices.Add(new DeviceInfoViewModel()
            {
                DeviceInfo = info,
                DeviceName = info?.DeviceName,
                StatusInfo = "Connected.",
                StatusColor = Brushes.Green
            });
        }
    }
    /// <summary>
    /// Saves all Devices to a .json File.
    /// </summary>
    private void SaveChanges()
    {
        string filePath = SystemPaths.GetBaseAppDataPath() + "devices.json";
        if(!File.Exists(filePath)) File.Create(filePath).Dispose();
        File.WriteAllText(filePath, JsonConvert.SerializeObject(_knownDevices)); 
    }

    private void TryToLoadSavedChanges()
    {
        string filePath = SystemPaths.GetBaseAppDataPath() + "devices.json";
        if (File.Exists(filePath))
        {
            List<DeviceInfo>? devices = JsonConvert.DeserializeObject<List<DeviceInfo>>(File.ReadAllText(filePath));
            if (devices != null)
            {
                _knownDevices = devices;
            }
        }
    }
}