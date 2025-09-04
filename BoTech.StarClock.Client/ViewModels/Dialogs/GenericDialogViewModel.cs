﻿using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media;
using BoTech.StarClock.Client.ViewModels;
using CherylUI.Controls;
using Material.Icons;
using Material.Icons.Avalonia;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Dialogs;

public class GenericDialogViewModel : DialogPageBase
{
    private Control _contentOfTheDialog;
    /// <summary>
    /// The Content presenter
    /// </summary>
    public Control ContentOfTheDialog
    {
        get => _contentOfTheDialog; 
        set => this.RaiseAndSetIfChanged(ref _contentOfTheDialog, value);
    }
    private MaterialIconKind _icon;
    /// <summary>
    /// The Icon of the dialog
    /// </summary>
    public MaterialIconKind Icon
    {
        get => _icon; 
        set => this.RaiseAndSetIfChanged(ref _icon, value);
    }

    private IBrush _iconColor = Brushes.White;
    public IBrush IconColor
    {
        get => _iconColor; 
        set =>  this.RaiseAndSetIfChanged(ref _iconColor, value);
    }
    /// <summary>
    /// All Buttons that are visible below the Content
    /// </summary>
    public List<DialogButton> DialogButtons { get; set;  } 
    
    public GenericDialogViewModel()
    {
        DialogButtons = new List<DialogButton>();
        _contentOfTheDialog = new TextBlock()
        {
            Text = "View Init."
        };
    }
    public GenericDialogViewModel(Control contentOfTheDialog)
    {
        DialogButtons = new List<DialogButton>();
        _contentOfTheDialog =  contentOfTheDialog;
    }
    public GenericDialogViewModel AddDialogButton(string buttonText, Action onClickAction, bool isEnabled = true)
    {
        DialogButtons.Add(new DialogButton()
        {
            ButtonText = buttonText,
            IsEnabled = isEnabled,
            OnClickCommand = ReactiveCommand.Create(onClickAction)
        });
        return this;
    }
   
}
public class DialogButton : ViewModelBase
{
    private bool _isEnabled = true;

    public bool IsEnabled
    {
        get => _isEnabled; 
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }
    public string ButtonText { get; set; } = String.Empty;
    public ReactiveCommand<Unit, Unit> OnClickCommand { get; set; }
}