using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Controls;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.Helper.ApiClient;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views.Dialog;
using BoTech.StarClock.Client.Views.Pages;
using CherylUI.Controls;
using Material.Icons;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class AddImageToSlideshowPageViewModel : ViewModelBase
{
    public ObservableCollection<SelectableImageViewModel> UploadedImages { get; set; } = new ObservableCollection<SelectableImageViewModel>();
    public ReactiveCommand<Unit, Unit> AddImagesToSlideShowCommand { get; set; }
    public List<SelectableImageViewModel> SelectedUploadedImages { get; set; } = new List<SelectableImageViewModel>();
    private ApiClient _client;
    private string _imageCachePath = SystemPaths.GetBaseAppDataPath("Cache/UploadedImages/");
    public AddImageToSlideshowPageViewModel(ApiClient apiClient)
    {
        _client = apiClient;
        AddImagesToSlideShowCommand = ReactiveCommand.Create(() =>
        {
            ProgressDialogViewModel.CreateProgressbarPage(AddImagesToSlideShow);
        });
        ReloadAllImages();
    }

    private void AddImagesToSlideShow(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, "Adding images to SlideShow", Brushes.Blue, MaterialIconKind.Loading);
        if (SelectedUploadedImages.Count == 1)
        {
            RequestResult<bool> result = _client.ImageSlideshowClient.AddImageToSlideshow(SelectedUploadedImages[0].LocalImage.Id.ToString());
            if (result.Success)
            {
                vm.Maximum = 100;
                vm.UpdateProgress(100, "Added all Images.", Brushes.Green, MaterialIconKind.Check);
                ClosePage();
                return;
            }
            vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        }
        else if (SelectedUploadedImages.Count > 1)
        {
            List<string> imageIds = new List<string>();
            foreach (SelectableImageViewModel uploadedImage in SelectedUploadedImages) imageIds.Add(uploadedImage.LocalImage.Id.ToString());
            RequestResult<bool> result = _client.ImageSlideshowClient.AddImageRangeToSlideshow(0, imageIds.ToArray());
            if (result.Success)
            {
                vm.Maximum = 100;
                vm.UpdateProgress(100, "Added all Images.", Brushes.Green, MaterialIconKind.Check);
                ClosePage();
                return;
            }
            vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        }
    }

    private void ClosePage()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            NavigationControl.Pop("NavigationControllerYourStarsTab");
        });
    }
    /// <summary>
    /// reloads all images from the star
    /// </summary>
    private void ReloadAllImages()
    {
        ProgressDialogViewModel.CreateProgressbarPage(ReloadAllImagesAction);
        //progressVm.OnPageShow();
    }
    /// <summary>
    ///  reloads all images from the star and injects it into the <see cref="UploadedImages"/> List
    /// </summary>
    /// <param name="data">Not used in this case</param>
    /// <param name="vm">The vm which controls the Progress bar.</param>
    /// <returns></returns>
    public void ReloadAllImagesAction(object? data, ProgressDialogViewModel vm)
    {
        UploadedImages.Clear();
        vm.UpdateProgressIntermediate(true, "Getting Images from Api...", Brushes.Blue, MaterialIconKind.Loading);
        vm.DisableDialogButton("Ok");
        RequestResult<List<LocalImage>> result = _client.ImageSlideshowClient.GetImageList();
        
        if (result.Success && result.ParsedData != null)
        {
            vm.Maximum = result.ParsedData.Count;
            vm.UpdateProgress(0,"Adding Images to the View and loading preview for Image.", Brushes.Blue, MaterialIconKind.Loading);
            foreach (var image in result.ParsedData)
            {
                vm.UpdateProgress(vm.Value + 1,$"Adding Images to the View and loading preview for Image {image.Id}", Brushes.Blue, MaterialIconKind.Loading);
                
                SelectableImageViewModel uploadedImage = new SelectableImageViewModel(this)
                {
                    LocalImage = image
                };
                AddImagePreviewToUploadedImage(uploadedImage, _client);
                UploadedImages.Add(uploadedImage);
            }
            vm.UpdateProgressIntermediate(false, $"Finished.", Brushes.Green, MaterialIconKind.Check);
            vm.EnableDialogButton("Ok");
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        vm.EnableDialogButton("Ok");
    }
    private void AddImagePreviewToUploadedImage(SelectableImageViewModel uploadedImage, ApiClient client)
    {
        // The image is initially saved to disk because the Avalonia bitmap cannot be created directly from System.Drawing.Bitmap.
        if(!Directory.Exists(_imageCachePath)) Directory.CreateDirectory(_imageCachePath);
        RequestResult<dynamic> result = client.ImageSlideshowClient.GetImagePreview(uploadedImage.LocalImage.Id.ToString(), 192,192, _imageCachePath + uploadedImage.LocalImage.Id + ".jpeg");
        if (result.Success)
        {
            string imagePath = Path.Combine(_imageCachePath, uploadedImage.LocalImage.Id.ToString() + ".jpeg");
            uploadedImage.Image = new Bitmap(imagePath);
            File.Delete(imagePath);
        }
    }
}