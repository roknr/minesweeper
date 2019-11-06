using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Minesweeper.DesktopApp.AttachedProperties
{
    /// <summary>
    /// Provides attached properties for <see cref="Frame"/>s.
    /// </summary>
    public static class FrameAttachedProperties
    {
        #region Disable frame navigation bar

        /// <summary>
        /// The <see cref="Frame"/> attached property that prevents the frame's
        /// navigation bar from being visible.
        /// </summary>
        public static readonly DependencyProperty DisableNavigationProperty =
            DependencyProperty.RegisterAttached(
                "DisableNavigation",
                typeof(bool),
                typeof(FrameAttachedProperties),
                new PropertyMetadata(false, DisableNavigationChanged));

        /// <summary>
        /// The <see cref="DisableNavigationProperty"/> get accessor.
        /// </summary>
        /// <param name="frame">The frame whose <see cref="DisableNavigationProperty"/>'s
        /// value to get.</param>
        /// <returns></returns>
        public static bool GetDisableNavigation(Frame frame)
        {
            return (bool)frame.GetValue(DisableNavigationProperty);
        }

        /// <summary>
        /// The <see cref="DisableNavigationProperty"/> set accessor.
        /// </summary>
        /// <param name="frame">The frame whose <see cref="DisableNavigationProperty"/>'s
        /// value to set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDisableNavigation(Frame frame, bool value)
        {
            frame.SetValue(DisableNavigationProperty, value);
        }

        /// <summary>
        /// The event handler for when the value of the <see cref="DisableNavigationProperty"/>'s value changes.
        /// Should only ever happen once per frame.
        /// </summary>
        /// <param name="sender">The frame on which the property changed.</param>
        /// <param name="e">The event arguments.</param>
        private static void DisableNavigationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // The sender is the frame
            var frame = (Frame)sender;

            // Prevent the frame from showing the navigation bar
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            frame.Navigated += (o, e) =>
            {
                ((Frame)o).NavigationService.RemoveBackEntry();
            };
        }

        #endregion
    }
}
