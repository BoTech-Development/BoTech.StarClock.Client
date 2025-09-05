using Avalonia.Media.Imaging;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Helper.ApiClient;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class SelectableImageViewModel
{
    public LocalImage LocalImage { get; set; }
    public Bitmap Image { get; set; }
    private bool _isSelected = false;
    public bool IsSelected
    {
        get =>  _isSelected;
        set
        {
            _isSelected = value;   
            NotifyVm();
        }
    }

    private AddImageToSlideshowPageViewModel _vm;
    public SelectableImageViewModel(AddImageToSlideshowPageViewModel vm)
    {
        _vm = vm;
    }

    private void NotifyVm()
    {
        if (IsSelected)
        {
            _vm.SelectedUploadedImages.Add(this);
        }
        else
        {
            _vm.SelectedUploadedImages.Remove(this);
        }
    }
}