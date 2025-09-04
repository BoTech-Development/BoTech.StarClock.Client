using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BoTech.StarClock.Client.Views.Pages;

public partial class ManageUploadedImagesPageView : UserControl
{
    private static ManageUploadedImagesPageView _instance;

    public static TopLevel? TopLevel
    {
        get =>  TopLevel.GetTopLevel(_instance);
    }
    public ManageUploadedImagesPageView()
    {
        _instance = this;
        InitializeComponent();
    }
}