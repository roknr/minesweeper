using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.DesktopApp.AttachedProperties;

/// <summary>
/// Provides attached properties for <see cref="UIElement"/>s.
/// </summary>
public static class UIElementAttachedProperties
{
    #region Enable switching focus to element on click

    /// <summary>
    /// The <see cref="UIElement"/> attached property that allows for switching focus to the element
    /// when clicked on it. Useful for attaching to containers like <see cref="Grid"/>. NOTE: the element's
    /// background property must be specified (it can be transparent too).
    /// </summary>
    public static readonly DependencyProperty FocusOnClickProperty =
        DependencyProperty.RegisterAttached(
            "FocusOnClick",
            typeof(bool),
            typeof(UIElementAttachedProperties),
            new PropertyMetadata(false, FocusOnClickChanged));

    /// <summary>
    /// The <see cref="FocusOnClickProperty"/> get accessor.
    /// </summary>
    /// <param name="element">The element whose <see cref="FocusOnClickProperty"/>'s value to get.</param>
    /// <returns></returns>
    public static bool GetFocusOnClick(UIElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return (bool)element.GetValue(FocusOnClickProperty);
    }

    /// <summary>
    /// The <see cref="FocusOnClickProperty"/> set accessor.
    /// </summary>
    /// <param name="element">The element whose <see cref="FocusOnClickProperty"/>'s value to set.</param>
    /// <param name="value">The value to set.</param>
    public static void SetFocusOnClick(UIElement element, bool value)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.SetValue(FocusOnClickProperty, value);
    }

    /// <summary>
    /// The event handler for when the value of the <see cref="FocusOnClickProperty"/>'s value changes.
    /// Should only ever happen once per element.
    /// </summary>
    /// <param name="sender">The element on which the property changed.</param>
    /// <param name="e">The event arguments.</param>
    private static void FocusOnClickChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // The sender is the UI element
        var element = (UIElement)sender;

        // Set it to be able to receive focus
        element.Focusable = true;

        // When the left mouse button is pressed on the element, attempt to focus it
        element.MouseLeftButtonDown += (o, e) => element.Focus();
    }

    #endregion
}
