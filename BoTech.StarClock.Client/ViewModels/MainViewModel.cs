using System;
using System.Collections.ObjectModel;
using System.Threading;
using Avalonia.Controls;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.ViewModels.Pages;
using BoTech.StarClock.Client.Views.Pages;

namespace BoTech.StarClock.Client.ViewModels;

public class MainViewModel : ViewModelBase
{
   
   public NavigationPageView NavigationPageView { get; set; }
    public MyStarsPageView MyStarsPageView { get; set; }
    public MainViewModel()
    {
        NavigationPageView = new NavigationPageView()
        {
            DataContext = new NavigationPageViewModel()
        };
        MyStarsPageView = new MyStarsPageView()
        {
            DataContext = new MyStarsPageViewModel()
        };
    }
}