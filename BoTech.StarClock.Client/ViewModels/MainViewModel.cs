using System;
using System.Threading;
using BoTech.StarClock.Client.Helper;

namespace BoTech.StarClock.Client.ViewModels;

public class MainViewModel : ViewModelBase
{
    

    public MainViewModel()
    {
      /*  BluetoothClientHelper client = new BluetoothClientHelper();
        Thread clientThread = new Thread(async () =>
        {
            await client.DiscoverAndConnectAsync();
            Thread readHandler = new Thread(() =>
            {
                while (true) client.ReceiveData();
            });
            Thread writeHandler = new Thread(() =>
            {
                string? input = null;
                while (true)
                {
                    input = Console.ReadLine();
                    if (input != null) client.SendData(input);
                }
            });
            writeHandler.Start();
            readHandler.Start();
        });
        clientThread.Start();*/
    }
}