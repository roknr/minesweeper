using Minesweeper.Core.Routing;
using Minesweeper.Core.Routing.Events;

namespace Minesweeper.Core.Interfaces.Routing;

/// <summary>
/// Defines the application router functionality.
/// </summary>
public interface IRouter
{
    #region Public methods

    /// <summary>
    /// Navigates to the specified route on the route's router depth with an optional argument.
    /// </summary>
    /// <param name="route">The destination route to navigate to.</param>
    /// <param name="argument">The optional argument to pass to the page (null by default).</param>
    void NavigateTo(Route route, object? argument = null);

    /// <summary>
    /// Gets the active route for the specified router depth. Throws an <see cref="ArgumentException"/>
    /// if no routes exist for the specified router depth.
    /// </summary>
    /// <param name="onDepth">The router depth for which to get the active route.</param>
    /// <returns></returns>
    Route GetActiveRoute(uint onDepth);

    #endregion

    #region Events

    /// <summary>
    /// The event that gets fired before the <see cref="IRouter"/> starts performing
    /// a navigation to a <see cref="Route"/>.
    /// </summary>
    event EventHandler<RouterNavigationCancelEventArgs> NavigationStarted;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> performs a successful
    /// navigation to a new route.
    /// </summary>
    event EventHandler<RouterNavigationEventArgs> NavigationCompleted;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> canceled the navigation
    /// to a <see cref="Route"/>.
    /// </summary>
    event EventHandler<RouterNavigationEventArgs> NavigationCanceled;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> encountered an error when
    /// navigating to a <see cref="Route"/>.
    /// </summary>
    event EventHandler<RouterNavigationErrorEventArgs> NavigationError;

    #endregion
}
