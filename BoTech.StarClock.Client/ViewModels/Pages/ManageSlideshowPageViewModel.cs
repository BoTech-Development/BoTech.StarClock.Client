using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Controls;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views.Dialog;
using BoTech.StarClock.Client.Views.Pages;
using CherylUI.Controls;
using Material.Icons;
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
    
    private DeviceInfoViewModel _deviceInfo;
    private string _imageCachePath = SystemPaths.GetBaseAppDataPath("Cache/UploadedImages/");
    public ManageSlideshowPageViewModel(DeviceInfoViewModel model)
    {
        _deviceInfo = model;
        SlideshowImages = new ObservableCollection<SlideshowImageViewModel>();
        ClearSlideshowCommand = ReactiveCommand.Create(ClearSlideshow);
        AddImageCommand = ReactiveCommand.Create(AddImageToSlideshow);
        LoadSlideshowImages();
    }

    private void AddImageToSlideshow()
    {
        NavigationControl.Push("NavigationControllerYourStarsTab", new AddImageToSlideshowPageView()
        {
            DataContext = new AddImageToSlideshowPageViewModel(_deviceInfo.DeviceInfo.ApiClient)
        });
    }
    private void ClearSlideshow()
    {
        RequestResult<bool> result = _deviceInfo.DeviceInfo.ApiClient.ImageSlideshowClient.ClearSlideshow();
    }
    public void LoadSlideshowImages()
    {
        ProgressDialogViewModel.CreateProgressbarPage(LoadSlideshowImagesAction);
    }

    private void LoadSlideshowImagesAction(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, "Getting Images from Api...", Brushes.Blue, MaterialIconKind.Loading);
        vm.DisableDialogButton("Ok");
        RequestResult<List<LocalImage>> result = _deviceInfo.DeviceInfo.ApiClient.ImageSlideshowClient.GetSlideshowImages();
        if (result.Success && result.ParsedData != null)
        {
            List<LocalImage> images = result.ParsedData;
            SlideshowImages.Clear();
            vm.UpdateProgress(0, "Getting Previews for Images...", Brushes.Blue, MaterialIconKind.Loading);
            vm.Maximum = images.Count;
            foreach (LocalImage image in images)
            {
                vm.UpdateProgress(vm.Value + 1, $"Getting Preview Image: {image.Id}", Brushes.Blue, MaterialIconKind.Loading);
                SlideshowImageViewModel viewModel = new SlideshowImageViewModel(_deviceInfo.DeviceInfo.ApiClient, this)
                {
                    ImageInfo = image,
                };
                if(!Directory.Exists(_imageCachePath)) Directory.CreateDirectory(_imageCachePath);
                RequestResult<dynamic> result2 = _deviceInfo.DeviceInfo.ApiClient.ImageSlideshowClient.GetImagePreview(image.Id.ToString(), 
                    60,
                    60, 
                    _imageCachePath + image.Id + ".jpeg");
                if (result2.Success)
                {
                    string imagePath = Path.Combine(_imageCachePath, image.Id.ToString() + ".jpeg");
                    viewModel.Image = new Bitmap(imagePath);
                    File.Delete(imagePath);
                }
                SlideshowImages.Add(viewModel);
            }
            vm.UpdateProgressIntermediate(false, $"Finished.", Brushes.Green, MaterialIconKind.Check);
            vm.EnableDialogButton("Ok");
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        vm.EnableDialogButton("Ok");
    }
}