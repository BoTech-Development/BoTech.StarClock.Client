using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Application = Android.App.Application;

namespace BoTech.StarClock.Client.Android;

public delegate void OnImagePickerAccepted(string pathToImage);
public delegate void OnImagePickerCanceled();
[Activity(
    Label = "BoTech.StarClock.Client.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    /*const int PICK_IMAGE = 1000;
    /// <summary>
    /// When the user selects a specific Image
    /// </summary>
    public static OnImagePickerAccepted? OnImagePickerAccepted;
    /// <summary>
    /// If the user aborted the Operation
    /// </summary>
    public static OnImagePickerAccepted? OnImagePickerCanceled; 
    /// <summary>
    /// Saving the instance is necessary to make the Method 
    /// </summary>
    private static MainActivity? Instance { get; set; }*/
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        //Instance = this;
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }
   /* protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        //PickImageFromGallery();
    }


    public static void PickImageFromGallery()
    {
        if(Instance != null) Instance.PickImageFormGalleryBase();
    }

    private void PickImageFormGalleryBase()
    {
        Intent intent = new Intent(Intent.ActionPick);
        intent.SetType("image/*");
        StartActivityForResult(intent, PICK_IMAGE);
    }
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode == PICK_IMAGE ) 
        {
            if (resultCode == Result.Ok && data != null)
            {
            }
            else if (resultCode == Result.Canceled && data == null)
            {
                
            }
            string imagePath = data.Data.Path;
           
            // You can now pass this URI to your Avalonia UI layer
        }
    }

*/
}