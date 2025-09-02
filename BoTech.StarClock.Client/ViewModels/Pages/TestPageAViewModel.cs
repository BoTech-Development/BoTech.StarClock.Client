using System.Reactive;
using BoTech.StarClock.Client.Controls;
using BoTech.StarClock.Client.Views.Pages;
using CherylUI.Controls;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class TestPageAViewModel : ViewModelBase
{
    //public MobileNavigation? NavigationControl { get; set; }
    public static int NumberOfPage { get; set; } = 0;
    public ReactiveCommand<Unit,Unit> ShowPageACommand { get; set; }
    public ReactiveCommand<Unit,Unit> ShowPageBCommand { get; set; }
    public ReactiveCommand<Unit,Unit> ShowPageCCommand { get; set; }
    public ReactiveCommand<Unit,Unit> BackCommand { get; set; }

    public TestPageAViewModel()
    {
        ShowPageACommand = ReactiveCommand.Create(() =>
        {
            NavigationControl.Push("NavigationControl", new TestPageAView()
            {
                DataContext = new TestPageAViewModel()
            });  
        });
        ShowPageBCommand = ReactiveCommand.Create(() =>
        {
            NavigationControl.Push("NavigationControl", new TestPageBView()
            {
                DataContext = new TestPageAViewModel()
            });  
        });
        ShowPageCCommand = ReactiveCommand.Create(() =>
        {
            NavigationControl.Push("NavigationControl", new TestPageCView()
            {
                DataContext = new TestPageAViewModel()
            });  
        });
        BackCommand = ReactiveCommand.Create(() =>
        {
            NavigationControl.Pop("NavigationControl");
        });
        NumberOfPage += 1;
    }
}