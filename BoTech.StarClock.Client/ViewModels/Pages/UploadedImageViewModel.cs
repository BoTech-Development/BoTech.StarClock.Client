using System.Collections.Generic;
using System.Net.Http;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using BoTech.StarClock.Api.SharedModels.Slideshow;
using BoTech.StarClock.Client.Helper;
using BoTech.StarClock.Client.Helper.ApiClient;
using BoTech.StarClock.Client.Models.ApiClient;
using BoTech.StarClock.Client.ViewModels.Dialogs;
using BoTech.StarClock.Client.Views.Dialog;
using CherylUI.Controls;
using Material.Icons;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Pages;

public class UploadedImageViewModel : ViewModelBase
{
    public LocalImage LocalImage { get; set; }
    public Bitmap Image { get; set; }
    public string ImageInfo { get; set; }
    public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }
    private ApiClient _client;
    private ManageUploadedImagesPageViewModel _manageUploadedImages;
    public UploadedImageViewModel(ApiClient client, ManageUploadedImagesPageViewModel manageUploadedImages)
    {
        _client = client;
        _manageUploadedImages = manageUploadedImages;
        DeleteImageCommand = ReactiveCommand.Create(DeleteImage);
    }
    /// <summary>
    /// Deletes this Image on the star
    /// </summary>
    private void DeleteImage()
    {
        ProgressDialogViewModel progressVm = new ProgressDialogViewModel();
        progressVm.Action = DeleteImageWithProgress;
        ProgressDialogView progressDialogView = new ProgressDialogView()
        {
            DataContext = progressVm
        };
        PagedDialog pagedDialog = new PagedDialog(
            new List<UserControl>()
            {
                progressDialogView,
            });
        pagedDialog.ShowPage(0);
        InteractiveContainer.ShowDialog(pagedDialog);
    }

    private void DeleteImageWithProgress(object data, ProgressDialogViewModel vm)
    {
        vm.IsIndeterminate = true;
        vm.ProgressText = $"Deleting image {LocalImage.Id}...";
        RequestResult<bool> result = _client.ImageSlideshowClient.DeleteImage(LocalImage.Id);
        if (result.Success && result.ParsedData != null)
        {
            _manageUploadedImages.ReloadAllImagesAction(data, vm);
            vm.ProgressText = "Finished!";
            vm.IsIndeterminate = false;
            vm.Maximum = 100;
            vm.Value = 100;
            vm.DialogSettings.Icon = MaterialIconKind.Check;
            return;
        }
        vm.ProgressText = "Error occured!";
        vm.IsIndeterminate = false;
        vm.Maximum = 100;
        vm.Value = 100;
        vm.DialogSettings.Icon = MaterialIconKind.Error;
    }
}