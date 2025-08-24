using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace BoTech.StarClock.Client.Helper;



public class BluetoothClientHelper
{
    private BluetoothClient _client;
    private BluetoothDeviceInfo? _device;

    public BluetoothClientHelper()
    {
        var radio = BluetoothRadio.Default;
        if (radio == null)
        {
            Console.WriteLine("Kein Bluetooth-Adapter gefunden.");
            return;
        }

        // Makes the device connectable (optional)
        radio.Mode = RadioMode.Connectable;

        /*var localAddress = radio.LocalAddress;
        _client = new BluetoothClient(
            new BluetoothEndPoint(localAddress,)
        );*/

    }
    public async Task DiscoverAndConnectAsync()
    {
        _client = new BluetoothClient();
        var devices = _client.DiscoverDevices();

        foreach (var device in devices)
        {
            Console.WriteLine($"Found device: {device.DeviceName} - {device.DeviceAddress}");
        }
        
        Console.WriteLine("Please enter device Name to connect to or 'r' to reload the list of all devices:");
        string? deviceNameToConnect = Console.ReadLine();
        if (deviceNameToConnect != null && deviceNameToConnect != "r")
        {
            if((_device = devices.Where(d => d.DeviceName == deviceNameToConnect).FirstOrDefault()) == null) return;
        }else if (deviceNameToConnect != null && deviceNameToConnect == "r")
        {
           await DiscoverAndConnectAsync();
           return;
        }else return;

        if (_device == null)
        {
            Console.WriteLine("Target device not found.");
            return;
        }

        Console.WriteLine("Connecting...");
        _client.Connect(_device.DeviceAddress, BluetoothService.SerialPort);
        Console.WriteLine("Connected!");
    }

    public void SendData(string message)
    {
        if (_client == null || !_client.Connected)
        {
            Console.WriteLine("Not connected to any device.");
            return;
        }

        var stream = _client.GetStream();
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
        Console.WriteLine("Data sent.");
    }

    public string ReceiveData()
    {
        if (_client == null || !_client.Connected)
        {
            Console.WriteLine("Not connected to any device.");
            return null;
        }

        var stream = _client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Received: {received}");
        return received;
    }
}
/*
public class BluetoothClient
{
    private readonly IBluetoothLE _ble;
    private readonly IAdapter _adapter;


    public BluetoothClient()
    {
        _ble = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;

    }
    public async Task ConnectAndSendAsync()
    {
        Console.WriteLine("🔍 Scanning for devices...");
        _adapter.DeviceDiscovered += async (s, deviceArgs) =>
        {
            Console.WriteLine($"📱 Found device: {deviceArgs.Device.Name}. Enter y to connect or n for searching for other devices.");
            string? selection = Console.ReadLine();
            if (selection != null && selection == "y")
            {
                try
                {
                    await _adapter.ConnectToDeviceAsync(deviceArgs.Device);
                    Console.WriteLine("✅ Connected!");

                    IService service = await deviceArgs.Device.GetServiceAsync(deviceArgs.Device.Id);
                    ICharacteristic characteristic = await service.GetCharacteristicAsync(service.Id);

                    if (characteristic.CanWrite && characteristic.CanRead)
                    {
                        Console.WriteLine("Can read and Write to: " + deviceArgs.Device.Name);
                        while (true)
                        {
                            (byte[] data, int resultCode) result = await characteristic.ReadAsync();
                            Console.WriteLine("Message received: " + Encoding.UTF8.GetString(result.data));
                        }
                        
                        
                        /*  byte[] data = Encoding.UTF8.GetBytes(message);
                          await characteristic.WriteAsync(data);
                          Console.WriteLine($"📤 Sent: {message}");*/
                   /* }
                }
                catch (DeviceConnectionException ex)
                {
                    Console.WriteLine($"❌ Connection failed: {ex.Message}");
                }
            }
        };

        await _adapter.StartScanningForDevicesAsync();
    }

}*/