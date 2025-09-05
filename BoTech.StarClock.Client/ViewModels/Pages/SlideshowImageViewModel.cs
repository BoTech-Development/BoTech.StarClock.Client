using System.Collections.Generic;
using System.Reactive;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Helper.ApiClient;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views.Dialog;
using CherylUI.Controls;
using Material.Icons;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class SlideshowImageViewModel : ViewModelBase 
{
    /// <summary>
    /// Image info about the Image
    /// </summary>
    public LocalImage ImageInfo { get; set; }
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
    private ManageSlideshowPageViewModel _manageSlideshowPageViewModel;

    private ApiClient _client;
    public SlideshowImageViewModel(ApiClient client, ManageSlideshowPageViewModel vm)
    {
        _manageSlideshowPageViewModel = vm;
        _client = client;
      
        RemoveImageCommand = ReactiveCommand.Create(() =>
        {
            ProgressDialogViewModel.CreateProgressbarPage(RemoveImage);
        });
        MoveImageBackwardCommand = ReactiveCommand.Create(() =>
        {
            ProgressDialogViewModel.CreateProgressbarPage(MoveImageBackward);
        });
        MoveImageForwardCommand = ReactiveCommand.Create(() =>
        {
            ProgressDialogViewModel.CreateProgressbarPage(MoveImageForward);
        });
        DuplicateImageCommand = ReactiveCommand.Create(() =>
        {
            ProgressDialogViewModel.CreateProgressbarPage(DuplicateImage);
        });
    }

    private void RemoveImage(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, $"Delete Image: {this.ImageInfo.Id}", Brushes.Blue, MaterialIconKind.Loading);
        int index = _manageSlideshowPageViewModel.SlideshowImages.IndexOf(this);
        if (index != -1)
        {
            RequestResult<bool> result = _client.ImageSlideshowClient.DeleteImageFromSlideshow(index);
            EnsureSuccess(result, vm);
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: This Image might not exist, \n in the Slideshow anymore.", Brushes.Orange, MaterialIconKind.Error);
    }
    private void MoveImageForward(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, $"Move Image Forward: {this.ImageInfo.Id}", Brushes.Blue, MaterialIconKind.Loading);
        int index = _manageSlideshowPageViewModel.SlideshowImages.IndexOf(this);
        if (index != -1)
        {
            RequestResult<bool> result = _client.ImageSlideshowClient.MoveImageTo(index, index - 1);
            EnsureSuccess(result, vm);
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: This Image might not exist, \n in the Slideshow anymore.", Brushes.Orange, MaterialIconKind.Error);
    }
    private void MoveImageBackward(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, $"Move Image Backward: {this.ImageInfo.Id}", Brushes.Blue, MaterialIconKind.Loading);
        int index = _manageSlideshowPageViewModel.SlideshowImages.IndexOf(this);
        if (index != -1)
        {
            RequestResult<bool> result = _client.ImageSlideshowClient.MoveImageTo(index, index + 1);
            EnsureSuccess(result, vm);
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: This Image might not exist, \n in the Slideshow anymore.", Brushes.Orange, MaterialIconKind.Error);
    }
    private void DuplicateImage(object? data, ProgressDialogViewModel vm)
    {
        vm.UpdateProgressIntermediate(true, $"Duplicating Image: {this.ImageInfo.Id}", Brushes.Blue, MaterialIconKind.Loading);
        int index = _manageSlideshowPageViewModel.SlideshowImages.IndexOf(this);
        if (index != -1)
        {
            RequestResult<bool> result = _client.ImageSlideshowClient.DuplicateImage(index);
            EnsureSuccess(result, vm);
            return;
        }
        vm.UpdateProgressIntermediate(false, $"Error: This Image might not exist, \n in the Slideshow anymore.", Brushes.Orange, MaterialIconKind.Error);
    }

    private void EnsureSuccess(RequestResult<bool> result, ProgressDialogViewModel vm)
    {
        if (!result.Success)
        {
            vm.UpdateProgressIntermediate(false, $"Error: {result.ResponseMessage}", Brushes.Orange, MaterialIconKind.Error);
        }
        else
        {
            ReloadView();
        }
    }
    private void ReloadView()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            InteractiveContainer.CloseDialog(); // Close Dialog for the next Dialog
            _manageSlideshowPageViewModel.LoadSlideshowImages();
        });
    }
}