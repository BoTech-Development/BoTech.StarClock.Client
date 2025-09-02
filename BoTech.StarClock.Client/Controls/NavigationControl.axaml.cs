using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using CherylUI.Controls;
using ReactiveUI;

namespace BoTech.StarClock.Client.Controls;

public partial class NavigationControl : UserControl
{
    public static readonly StyledProperty<string> PageNameProperty =
        AvaloniaProperty.Register<NavigationControl, string>(nameof(PageName));
    /// <summary>
    /// The name of the page which will be displayed at the top next to the arrow button.
    /// </summary>
    public string PageName
    {
        get => GetValue(PageNameProperty);
        set => SetValue(PageNameProperty, value);
    }
    
    private Stack<Control> _pages = new Stack<Control>();
    /// <summary>
    /// The Page that the View shows at the moment. <br/>
    /// This var will be not null when the user invoked <see cref="Push"/> without any errors.
    /// </summary>
    public Control? CurrentPage { get; private set; } = null;
    public NavigationControl()
    {
        InitializeComponent();
    }

    private void AnimateAndUpdateContent(Control newPage)
    {
        TransitioningContentControl? animatableContentProvider =this.GetTemplateChildren().OfType<TransitioningContentControl>().FirstOrDefault();
        if(animatableContentProvider != null)
            animatableContentProvider.Content = newPage;
        //this.AnimatableContentProvider.Content = newPage;
    }

    public static void Pop(string controlName)
    {
        NavigationControl? controller = TryToGetNavigationControllerInstance(controlName);
        if (controller != null)
        {
            // Do nothing when the user invokes the method but there are no recent pages.
            if (controller._pages.Count == 0)
                return;
            // Get the last page.
            Control page = controller._pages.Pop();
            controller.CurrentPage = page;

            // Animate and update the content:
            controller.AnimateAndUpdateContent(page);
            //TransitioningContentControl animatableContentProvider = (TransitioningContentControl)controller.GetTemplateChildren().First(f => f.Name == "AnimatableContentProvider");
            //animatableContentProvider.Content = newPage;
        }
    }
    /// <summary>
    /// Pushes a new Page to the Stack and show it
    /// </summary>
    /// <param name="controlName">The name of the Control <see cref="NavigationControl"/> </param>
    /// <param name="newPage">The new page that should be shown by the Navigation Controller.</param>
    public static void Push(string controlName, UserControl newPage)
    {
        NavigationControl? controller = TryToGetNavigationControllerInstance(controlName);
        if (controller != null)
        {
            if (controller.CurrentPage == null && controller.Content != null)
                controller.CurrentPage = (Control)controller.Content;
            if(controller.CurrentPage != null)
                controller._pages.Push(controller.CurrentPage);
            controller.CurrentPage = newPage;

            // Animate and update the content:
            controller.AnimateAndUpdateContent(newPage);
            //TransitioningContentControl animatableContentProvider = (TransitioningContentControl)controller.GetTemplateChildren().First(f => f.Name == "AnimatableContentProvider");
            //animatableContentProvider.Content = newPage;
        }
    }
    /// <summary>
    /// Returns the navigation Controller relative to the given name.
    /// This Method searches 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static NavigationControl? TryToGetNavigationControllerInstance(string name)
    {
        NavigationControl? container = null;
        try
        {
            if (Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime)
            {
                container = ((ISingleViewApplicationLifetime)Application.Current.ApplicationLifetime).MainView
                    .GetVisualDescendants()
                    .OfType<NavigationControl>()
                    .Where(n => n.Name == name)
                    .FirstOrDefault();
            }
            else if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                container = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime)
                    .MainWindow.GetVisualDescendants()
                    .OfType<NavigationControl>()
                    .Where(n => n.Name == name)
                    .FirstOrDefault();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return container;
    }

    private void OnBackButtonClick(object? sender, RoutedEventArgs e)
    {
        NavigationControl.Pop(this.Name);
    }
}