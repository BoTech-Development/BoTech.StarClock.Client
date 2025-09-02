using System;
using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Application = Android.App.Application;

namespace BoTech.StarClock.Client.Android;

[Activity(
    Label = "BoTech.StarClock.Client.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string personal = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string commonApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string resources = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
        string commonDocuments = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        //Console.WriteLine(Application.Context.GetExternalFilesDir(null).AbsolutePath);
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }
}