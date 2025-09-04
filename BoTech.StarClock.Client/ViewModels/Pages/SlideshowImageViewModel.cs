using System.Reactive;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class SlideshowImageViewModel : ViewModelBase
{
    /// <summary>
    /// The Image
    /// </summary>
    public Bitmap Image { get; set; }
    /// <summary>
    /// Move the Image to the start point of the Timeline
    /// </summary>
    public ReactiveCommand<Unit, Unit> MoveImageForwardCommand { get; }
    /// <summary>
    /// Move the Image to the end of the Timeline
    /// </summary>
    public ReactiveCommand<Unit, Unit> MoveImageBackwardCommand { get; }
    /// <summary>
    /// Clone the Image and inject it before this Image in the Timeline.
    /// </summary>
    public ReactiveCommand<Unit, Unit> DuplicateImageCommand { get; }
    /// <summary>
    /// Remove the Image from the Timeline.
    /// </summary>
    public ReactiveCommand<Unit, Unit> RemoveImageCommand { get; }
}