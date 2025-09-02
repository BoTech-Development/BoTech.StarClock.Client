using BoTech.StarClock.Client.Views.Pages;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class NavigationPageViewModel : ViewModelBase
{
    public TestPageAView FirstView {get; set;}

    public NavigationPageViewModel()
    {
        FirstView = new TestPageAView()
        {
            DataContext = new TestPageAViewModel()
        };
    }
}