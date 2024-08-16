using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing.Events;

namespace Minesweeper.DesktopApp.Routing;

/// <summary>
/// The router control that simplifies application routing. Acts as a placeholder that
/// will be dynamically filled based on the application router's state.
/// </summary>
public partial class RouterControl : UserControl
{
    #region Private members

    /// <summary>
    /// The application router.
    /// </summary>
    /// <remarks>
    /// Ignore nullable error because it will always be set if not in design mode.
    /// </remarks>
    private readonly IRouter _router = null!;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="RouterControl"/> class.
    /// </summary>
    public RouterControl()
    {
        InitializeComponent();

#pragma warning disable RCS1208
        // Only set the router if not in design mode because in design mode IoC is not set up and a binding will not exist
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            _router = Core.IoC.Get<IRouter>();
            _router.NavigationCompleted += Router_NavigationCompleted;
        }
#pragma warning restore RCS1208
    }

    #endregion

    #region Event handlers

    /// <summary>
    /// The event handler for when the application router successfully performs a navigation to a new route.
    /// </summary>
    /// <param name="sender">The application router.</param>
    /// <param name="e">The router's on successfully navigated event arguments.</param>
    private void Router_NavigationCompleted(object? sender, RouterNavigationEventArgs e)
    {
        // Ignore updates in active routes for depths other that this router's depth
        if (e.Destination.Depth != Depth)
        {
            return;
        }

        // If the active route on this router's depth was updated:
        // Get this router control frame's content property and force update the binding, even though the bound
        // value (Depth property) will be the same, it will force a refresh from inside the depth-to-route value
        // converter that will yield the newly selected page for this depth.
        var contentBinding = mRouterFrame.GetBindingExpression(ContentProperty);
        contentBinding.UpdateTarget();
    }

    /// <summary>
    /// The event handler for when this router control is unloaded.
    /// </summary>
    /// <param name="sender">The router control (this).</param>
    /// <param name="e">The event arguments.</param>
    private void RouterControl_Unloaded(object sender, RoutedEventArgs e)
    {
        // Unregister the router's on navigated event handler when this router control gets destroyed
        _router.NavigationCompleted -= Router_NavigationCompleted;
    }

    #endregion

    #region Dependency properties

    /// <summary>
    /// The depth at which this router control display the pages.
    /// </summary>
    public uint Depth
    {
        get => (uint)GetValue(DepthProperty);
        set => SetValue(DepthProperty, value);
    }

    /// <summary>
    /// The dependency property for the <see cref="Depth"/> property so that the depth can be specified from
    /// within XAML. Initial (default) depth if not specified is 0. This dependency property does not support
    /// data binding, since the depth value should only be set once.
    /// </summary>
    public static readonly DependencyProperty DepthProperty =
        DependencyProperty.Register(
            nameof(Depth),
            typeof(uint),
            typeof(RouterControl),
            new FrameworkPropertyMetadata((uint)0, FrameworkPropertyMetadataOptions.NotDataBindable));

    #endregion
}
