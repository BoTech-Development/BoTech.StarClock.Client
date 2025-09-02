using System;
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
   /* private bool _isHelpOptionEnabled;
    public bool IsHelpOptionEnabled
    {
        get => _isHelpOptionEnabled;
        set =>  this.RaiseAndSetIfChanged(ref _isHelpOptionEnabled, value);
    }
    private bool _isSaveOptionEnabled;
    public bool IsSaveOptionEnabled 
    {
        get => _isSaveOptionEnabled;
        set => this.RaiseAndSetIfChanged(ref _isSaveOptionEnabled, value); 
    }
    private bool _isCloseOptionEnabled;
    public bool IsCloseOptionEnabled
    {
        get => _isCloseOptionEnabled;
        set => this.RaiseAndSetIfChanged(ref _isCloseOptionEnabled, value);
    }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
    public ReactiveCommand<Unit, Unit> HelpCommand { get; set; }*/

    public GenericDialogViewModel()//(List<DialogButton> dialogButtons)
    {
        //DialogButtons = dialogButtons;
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