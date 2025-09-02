using System;
using System.Collections.Generic;
using Avalonia.Controls;
using BoTech.StarClock.Client.Views.Dialogs;
using CherylUI.Controls;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Dialogs;
public delegate void OnDialogAccepted();
public delegate void OnDialogClosed();
public delegate void OnDialogInitialized(PagedDialog dialog);

/// <summary>
/// A control which can be used to show multiple page by using a next and previous button.
/// </summary>
public class PagedDialog : ContentControl
{
    /// <summary>
    /// Is called no matter which dialog was initialized
    /// </summary>
    public static OnDialogInitialized? OnDialogInitialized {get; set;}

    /// <summary>
    /// Event will be invoked, when the user click the ok button on the last page
    /// </summary>
    public OnDialogAccepted? OnDialogAccepted {get; set;}
    /// <summary>
    /// Event will be invoked, when the user clicks the cancel button on the first or last page
    /// </summary>
    public OnDialogAccepted? OnDialogCanceled {get; set;}
    /// <summary>
    /// A Model which could contain all inputs that the user makes on specific pages.
    /// </summary>
    public object DialogDataModel {get; set;}
    /// <summary>
    /// All Views that can be displayed as pages.
    /// it is necessary to store all GenericDialogViews, because in this case the visual parent fill be the same. => No avalonia error
    /// </summary>
    private List<GenericDialogView> _pageViews = new List<GenericDialogView>();

    private int _currentPageIndex;

    public PagedDialog(List<UserControl> pageViews)
    {
        InitPages(pageViews);
        if(OnDialogInitialized != null)
            OnDialogInitialized.Invoke(this);
    }

    private void InitPages(List<UserControl> pageViews)
    {
         int pageNumber = 0;
        foreach (var pageView in pageViews)
        {
            
            GenericDialogViewModel genericDialogViewModel;
            // Is First Page?
            if (pageNumber == 0)
            {
                pageNumber++;
                genericDialogViewModel = new GenericDialogViewModel()
                {
                    DialogButtons = new List<DialogButton>()
                    {
                        new DialogButton()
                        {
                            ButtonText = "Cancel",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                if (OnDialogCanceled != null)
                                    OnDialogCanceled.Invoke();
                                InteractiveContainer.CloseDialog();
                            })
                        },
                        new DialogButton()
                        {
                            ButtonText = "Next",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                _currentPageIndex++;
                                ShowPage(_currentPageIndex);
                            })
                        }
                    },
                    ContentOfTheDialog = pageView,
                };

                // Is last Page?
            } else if (pageNumber == pageViews.Count - 1)
            {
                pageNumber++;
                genericDialogViewModel = new GenericDialogViewModel()
                {
                    DialogButtons = new List<DialogButton>()
                    {
                        new DialogButton()
                        {
                            ButtonText = "Cancel",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                if (OnDialogCanceled != null)
                                    OnDialogCanceled.Invoke();
                                InteractiveContainer.CloseDialog();
                            })
                        },
                        new DialogButton()
                        {
                            ButtonText = "Ok",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                if (OnDialogAccepted != null)
                                    OnDialogAccepted.Invoke();
                                InteractiveContainer.CloseDialog();
                            })
                        }
                    },
                    ContentOfTheDialog = pageView,
                };
            }
            else // Some page in the middle => previous and next
            {
                pageNumber++;
                genericDialogViewModel = new GenericDialogViewModel()
                {
                    DialogButtons = new List<DialogButton>()
                    {
                        new DialogButton()
                        {
                            ButtonText = "Previous",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                _currentPageIndex--;
                                ShowPage(_currentPageIndex);
                            })
                        },
                        new DialogButton()
                        {
                            ButtonText = "Next",
                            OnClickCommand = ReactiveCommand.Create(() =>
                            {
                                _currentPageIndex++;
                                ShowPage(_currentPageIndex);
                            })
                        }
                    },
                    ContentOfTheDialog = pageView,
                };
            }

            if (pageView.DataContext is DialogPageBase vm)
            {
                vm.Dialog = this;
                vm.DialogSettings = genericDialogViewModel; // Make it reachable from each Vm.
            }
            _pageViews.Add(new GenericDialogView()
            {
                DataContext = genericDialogViewModel,
            });
        }
    }
    public void ShowPage(int pageNumber)
    {
        if (0 <= pageNumber && pageNumber < _pageViews.Count)
        {
            Content = _pageViews[pageNumber];
            if(_pageViews[pageNumber].DataContext is GenericDialogViewModel genericDialogViewModel)
            {
                if (genericDialogViewModel.ContentOfTheDialog != null)
                {
                    if (genericDialogViewModel.ContentOfTheDialog.DataContext is DialogPageBase vm)
                    {
                        vm.OnPageShow();
                    }
                }
            }
        }
    }
}
