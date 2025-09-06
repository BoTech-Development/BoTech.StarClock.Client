using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.Helper.ApiClient;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views;
using BoTech.StarClock.Client.Views.Dialog;
using BoTech.StarClock.Client.Views.Pages;
using CherylUI.Controls;
using Material.Icons;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class ManageUploadedImagesPageViewModel
{
    public ObservableCollection<UploadedImageViewModel> UploadedImages { get; set; } = new ObservableCollection<UploadedImageViewModel>();
    public ReactiveCommand<Unit, Unit> UploadImageCommand { get; set; }
    /// <summary>
    /// necessary to access the api.
    /// </summary>
    private DeviceInfoViewModel _deviceInfo;
    private string _imageCachePath = SystemPaths.GetBaseAppDataPath("Cache/UploadedImages/");
    public ManageUploadedImagesPageViewModel(DeviceInfoViewModel model)
    {
        _deviceInfo = model;
        UploadImageCommand = ReactiveCommand.Create(AddNewImage);
        ReloadAllImages();
    }
    /// <summary>
    /// reloads all images from the star
    /// </summary>
    private void ReloadAllImages()
    {
        ProgressDialogViewModel.CreateProgressbarPage(ReloadAllImagesAction);
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
        RequestResult<List<LocalImage>> result = _deviceInfo.DeviceInfo.ApiClient.ImageSlideshowClient.GetImageList();
        
        if (result.Success && result.ParsedData != null)
        {
            vm.Maximum = result.ParsedData.Count;
            vm.UpdateProgress(0,"Adding Images to the View and loading preview for Image.", Brushes.Blue, MaterialIconKind.Loading);
            foreach (var image in result.ParsedData)
            {
                vm.UpdateProgress(vm.Value + 1,$"Adding Images to the View and loading preview for Image {image.Id}", Brushes.Blue, MaterialIconKind.Loading);
                
                UploadedImageViewModel uploadedImage = new UploadedImageViewModel(_deviceInfo.DeviceInfo.ApiClient, this)
                {
                    ImageInfo = image.Id.ToString(),
                    LocalImage = image
                };
                AddImagePreviewToUploadedImage(uploadedImage, _deviceInfo.DeviceInfo.ApiClient);
                UploadedImages.Add(uploadedImage);
            }
            vm.UpdateProgressIntermediate(false, $"Finished.", Brushes.Green, MaterialIconKind.Check);
            vm.EnableDialogButton("Ok");
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        vm.EnableDialogButton("Ok");
        
    }

    private void AddImagePreviewToUploadedImage(UploadedImageViewModel uploadedImage, ApiClient client)
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
    
    private void AddNewImage()
    {
        TopLevel? topLevel = ManageUploadedImagesPageView.TopLevel;
       // IStorageProvider? provider = MainWindow.StorageProviderInstance;
        if (topLevel != null)
        {
            Thread addNewImageThread = new Thread(() =>
            {
                List<IStorageFile> files = topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = "Please select an image to upload.",
                    AllowMultiple = true,
                    FileTypeFilter = [FilePickerFileTypes.ImageAll],
                }).Result.ToList();
                foreach (IStorageFile file in files)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Stream stream = file.OpenReadAsync().Result;
                        stream.CopyTo(ms);
                        RequestResult<string> result = _deviceInfo.DeviceInfo.ApiClient.ImageSlideshowClient.UploadImage(file.Path.AbsolutePath, ms.ToArray());
                        if (result.Success)
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                // Update the View.
                                ReloadAllImages();
                            });
                        }
                    }
                }
            });
            addNewImageThread.Start();
        }
    }
}