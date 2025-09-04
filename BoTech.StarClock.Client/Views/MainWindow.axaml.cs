using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace BoTech.StarClock.Client.Views;

public partial class MainWindow : Window
{
    /// <summary>
    /// Returns the Storage Provicer instance which will be injected in the MainWindow by avalonia.
    /// </summary>
    public static IStorageProvider? StorageProviderInstance
    {
        get
        {
            if(_instance != null)
                return _instance.GetStorageProvider();
            else
                return null;
        }
    }

    private static MainWindow? _instance;
    public MainWindow()
    {
        _instance = this;
        InitializeComponent();
    }
    private IStorageProvider GetStorageProvider() => StorageProvider;
}