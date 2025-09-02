using System;
using ReactiveUI;

namespace BoTech.StarClock.Client.ViewModels.Dialogs;

public class ProgressDialogViewModel : DialogPageBase
{
    private string _progressText = "View Init. Please wait...";
    /// <summary>
    /// Text above the ProgressBar
    /// </summary>
    public string ProgressText
    {
        get => _progressText; 
        set => this.RaiseAndSetIfChanged(ref _progressText, value);
    }
    private int _minimum = 0;
    /// <summary>
    /// Progressbar minimum value
    /// </summary>
    public int Minimum
    {
        get => _minimum; 
        set => this.RaiseAndSetIfChanged(ref _minimum, value);
    }
    private int _maximum = 100;
    /// <summary>
    /// Progressbar Maximum value => default is 100%
    /// </summary>
    public int Maximum
    {
        get => _maximum; 
        set => this.RaiseAndSetIfChanged(ref _maximum, value);
    }
    private int _value = 0;
    /// <summary>
    /// Progressbar Value value
    /// </summary>
    public int Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);
            if(_value == Maximum) UpdateDialogButtons(true);
        }
    }

    private bool _isIndeterminate = false;
    /// <summary>
    /// Progressbar Value value
    /// </summary>
    public bool IsIndeterminate
    {
        get => _isIndeterminate; 
        set => this.RaiseAndSetIfChanged(ref _isIndeterminate, value);
    }
    /// <summary>
    /// The action that should be executed on Show. <br/>
    /// return value is not processed
    /// </summary>
    public Func<object, ProgressDialogViewModel, bool> Action { get; set; }
    public ProgressDialogViewModel()
    {
        PagedDialog.OnDialogInitialized += OnDialogInitialized;
        
    }

    public override void OnPageShow()
    {
        if(Dialog != null)
            Action.Invoke(Dialog.DialogDataModel, this);
        else
            Action.Invoke("", this);
    }
    private void OnDialogInitialized(PagedDialog dialog)
    {
        UpdateDialogButtons(false);
    }
    /// <summary>
    /// When the progress value is equals to Maximum, all buttons will be enabled.
    /// </summary>
    private void UpdateDialogButtons(bool areButtonsEnabled)
    {
        if(this.DialogSettings != null)
            foreach (DialogButton button in this.DialogSettings.DialogButtons)
                button.IsEnabled = areButtonsEnabled;
    }
}