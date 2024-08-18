using System.Windows;
using System.Windows.Input;

namespace Minesweeper.DesktopApp.AttachedProperties;

public static class MouseButtonsAttachedProperties
{
    #region Middle mouse button down command

    public static readonly DependencyProperty MiddleMouseButtonDownCommandProperty =
        DependencyProperty.RegisterAttached(
            "MiddleMouseButtonDownCommand",
            typeof(ICommand),
            typeof(MouseButtonsAttachedProperties),
            new PropertyMetadata(null, OnMiddleMouseButtonDownCommandChanged));

    public static ICommand GetMiddleMouseButtonDownCommand(DependencyObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return (ICommand)obj.GetValue(MiddleMouseButtonDownCommandProperty);
    }

    public static void SetMiddleMouseButtonDownCommand(DependencyObject obj, ICommand value)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.SetValue(MiddleMouseButtonDownCommandProperty, value);
    }

    private static void OnMiddleMouseButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        element.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
        element.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(OnMouseEnter));
    }

    #endregion

    #region Middle mouse button up command

    public static readonly DependencyProperty MiddleMouseButtonUpCommandProperty =
        DependencyProperty.RegisterAttached(
            "MiddleMouseButtonUpCommand",
            typeof(ICommand),
            typeof(MouseButtonsAttachedProperties),
            new PropertyMetadata(null, OnMiddleMouseButtonUpCommandChanged));

    public static ICommand GetMiddleMouseButtonUpCommand(DependencyObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return (ICommand)obj.GetValue(MiddleMouseButtonUpCommandProperty);
    }

    public static void SetMiddleMouseButtonUpCommand(DependencyObject obj, ICommand value)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.SetValue(MiddleMouseButtonUpCommandProperty, value);
    }

    private static void OnMiddleMouseButtonUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        element.AddHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(OnMouseUp));
        element.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(OnMouseLeave));
    }

    #endregion

    #region Right mouse button click command

    public static readonly DependencyProperty RightMouseButtonClickCommandProperty =
        DependencyProperty.RegisterAttached(
            "RightMouseButtonClickCommand",
            typeof(ICommand),
            typeof(MouseButtonsAttachedProperties),
            new PropertyMetadata(null, OnRightMouseButtonClickCommandChanged));

    public static ICommand GetRightMouseButtonClickCommand(DependencyObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return (ICommand)obj.GetValue(RightMouseButtonClickCommandProperty);
    }

    public static void SetRightMouseButtonClickCommand(DependencyObject obj, ICommand value)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.SetValue(RightMouseButtonClickCommandProperty, value);
    }

    private static void OnRightMouseButtonClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UIElement element)
        {
            return;
        }

        element.AddHandler(UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(OnMouseDown));
    }

    #endregion

    #region Mouse event handlers

    private static void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        var command = e.ChangedButton switch
        {
            MouseButton.Right => GetRightMouseButtonClickCommand(element),
            MouseButton.Middle => GetMiddleMouseButtonDownCommand(element),
            _ => null
        };

        if (command?.CanExecute(null) != true)
        {
            return;
        }

        command.Execute(null);
        e.Handled = true;
    }

    private static void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        var command = e.ChangedButton switch
        {
            MouseButton.Middle => GetMiddleMouseButtonUpCommand(element),
            _ => null
        };

        if (command?.CanExecute(null) != true)
        {
            return;
        }

        command.Execute(null);
        e.Handled = true;
    }

    private static void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        var command = e.MiddleButton is MouseButtonState.Pressed
            ? GetMiddleMouseButtonUpCommand(element)
            : null;

        if (command?.CanExecute(null) != true)
        {
            return;
        }

        command.Execute(null);
    }

    private static void OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (sender is not UIElement element)
        {
            return;
        }

        var command = e.MiddleButton is MouseButtonState.Pressed
            ? GetMiddleMouseButtonDownCommand(element)
            : null;

        if (command?.CanExecute(null) != true)
        {
            return;
        }

        command.Execute(null);
    }

    #endregion
}
