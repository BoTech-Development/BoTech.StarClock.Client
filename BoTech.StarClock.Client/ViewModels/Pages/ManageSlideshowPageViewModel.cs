using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class ManageSlideshowPageViewModel
{
    /// <summary>
    /// All Images in the Slideshow Timeline
    /// </summary>
    public ObservableCollection<SlideshowImageViewModel> SlideshowImages { get; }
    public ReactiveCommand<Unit, Unit> AddImageCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSlideshowCommand { get; }
    public ManageSlideshowPageViewModel(DeviceInfoViewModel model)
    {
        SlideshowImages = new ObservableCollection<SlideshowImageViewModel>();
    }
}