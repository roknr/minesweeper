using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Minesweeper.DesktopApp.AttachedProperties;

/// <summary>
/// Provides attached properties for <see cref="Slider"/>s.
/// </summary>
public static class SliderAttachedProperties
{
    #region Enable slider click-drag

    /// <summary>
    /// The <see cref="Slider"/> attached property that allows the slider to be click-dragable.
    /// </summary>
    public static readonly DependencyProperty ClickDragProperty =
        DependencyProperty.RegisterAttached(
            "ClickDrag",
            typeof(bool),
            typeof(SliderAttachedProperties),
            new PropertyMetadata(false, ClickDragChanged));

    /// <summary>
    /// The <see cref="ClickDragProperty"/> get accessor.
    /// </summary>
    /// <param name="slider">The slider whose <see cref="ClickDragProperty"/>'s value to get.</param>
    /// <returns></returns>
    public static bool GetClickDrag(Slider slider)
    {
        ArgumentNullException.ThrowIfNull(slider);

        return (bool)slider.GetValue(ClickDragProperty);
    }

    /// <summary>
    /// The <see cref="ClickDragProperty"/> set accessor.
    /// </summary>
    /// <param name="slider">The slider whose <see cref="ClickDragProperty"/>'s value to set.</param>
    /// <param name="value">The value to set.</param>
    public static void SetClickDrag(Slider slider, bool value)
    {
        ArgumentNullException.ThrowIfNull(slider);

        slider.SetValue(ClickDragProperty, value);
    }

    /// <summary>
    /// The event handler for when the value of the <see cref="ClickDragProperty"/>'s value changes.
    /// Should only ever happen once per slider.
    /// </summary>
    /// <param name="sender">The slider on which the property changed.</param>
    /// <param name="e">The event arguments.</param>
    private static void ClickDragChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // The sender is the slider
        var slider = (Slider)sender;
        slider.IsMoveToPointEnabled = true;

        // We need to wait for the slider to be loaded
        slider.Loaded += Slider_Loaded;
    }

    /// <summary>
    /// The event handler for when the slider is loaded. Needed because we're accessing the slider
    /// template which is set after the slider is loaded.
    /// </summary>
    /// <param name="sender">The slider.</param>
    /// <param name="e">The event arguments.</param>
    private static void Slider_Loaded(object sender, RoutedEventArgs e)
    {
        // The sender is the slider
        var slider = (Slider)sender;

        // Get the slider thumb and handle the thumb mouse enter event
        var thumb = ((Track)slider.Template.FindName("PART_Track", slider)).Thumb;

        thumb.MouseEnter += (o, thumbMouseEventArgs) =>
        {
            // If the left mouse button is not pressed, do nothing as the thumb shouldn't move
            if (thumbMouseEventArgs.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            // Otherwise raise the appropriate event that will move the slider thumb
            var eventArgs = new MouseButtonEventArgs(thumbMouseEventArgs.MouseDevice, thumbMouseEventArgs.Timestamp, MouseButton.Left)
            {
                RoutedEvent = UIElement.MouseLeftButtonDownEvent
            };

            thumb.RaiseEvent(eventArgs);
        };

        // Remove the loaded event handler since it needs to be executed only once
        slider.Loaded -= Slider_Loaded;
    }

    #endregion
}
