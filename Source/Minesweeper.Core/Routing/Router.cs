using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing.Enums;
using Minesweeper.Core.Routing.Events;

namespace Minesweeper.Core.Routing;

/// <summary>
/// The application router.
/// </summary>
public class Router : IRouter
{
    #region Private members

    /// <summary>
    /// All of the application routes.
    /// </summary>
    private readonly IEnumerable<Route> _routes;

    /// <summary>
    /// The selected routes on each depth.
    /// </summary>
    private readonly Dictionary<uint, Route> _activeRoutes = [];

    #endregion

    #region Events

    /// <summary>
    /// The event that gets fired before the <see cref="IRouter"/> starts performing
    /// a navigation to a <see cref="Route"/>.
    /// </summary>
    public event EventHandler<RouterNavigationCancelEventArgs>? NavigationStarted;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> performs a successful
    /// navigation to a new route.
    /// </summary>
    public event EventHandler<RouterNavigationEventArgs>? NavigationCompleted;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> canceled the navigation
    /// to a <see cref="Route"/>.
    /// </summary>
    public event EventHandler<RouterNavigationEventArgs>? NavigationCanceled;

    /// <summary>
    /// The event that gets fired after the <see cref="IRouter"/> encountered an error when
    /// navigating to a <see cref="Route"/>.
    /// </summary>
    public event EventHandler<RouterNavigationErrorEventArgs>? NavigationError;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Router"/> class.
    /// </summary>
    /// <param name="routes">The application routes.</param>
    public Router(IReadOnlyCollection<Route> routes)
    {
        ArgumentNullException.ThrowIfNull(routes);

        ValidateAndHandleRoutes(routes);
        _routes = routes;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Gets the active route for the specified router depth. Throws an <see cref="ArgumentException"/>
    /// if no routes exist for the specified router depth.
    /// </summary>
    /// <param name="onDepth">The router depth for which to get the active route.</param>
    public Route GetActiveRoute(uint onDepth)
    {
        // Validate that at least one route exists for the specified depth
        return !_activeRoutes.TryGetValue(onDepth, out var activeRoute)
            ? throw new ArgumentException("No routes exist for the specified router depth.", nameof(onDepth))
            : activeRoute;
    }

    /// <summary>
    /// Navigates to the specified route on the route's router depth with an optional argument.
    /// </summary>
    /// <param name="route">The destination route to navigate to.</param>
    /// <param name="argument">The optional argument to pass to the page (null by default).</param>
    public void NavigateTo(Route route, object? argument = null)
    {
        ArgumentNullException.ThrowIfNull(route);

        // Fire off the navigation started event
        var navigationStartedEventArgs = new RouterNavigationCancelEventArgs(route);
        NavigationStarted?.Invoke(this, navigationStartedEventArgs);

        // Cancel the navigation if the event was handled to do so
        if (navigationStartedEventArgs.Cancel)
        {
            NavigationCanceled?.Invoke(this, new RouterNavigationEventArgs(route));
            return;
        }

        // If the specified route doesn't exist or it's already active, raise the navigation error event and cancel the navigation
        if (!_routes.Contains(route))
        {
            NavigationError?.Invoke(this, new RouterNavigationErrorEventArgs(route, RouterNavigationError.NonExistingRoute));
            return;
        }
        else if (_activeRoutes[route.Depth] == route)
        {
            NavigationError?.Invoke(this, new RouterNavigationErrorEventArgs(route, RouterNavigationError.RouteAlreadyActive));
            return;
        }

        // Otherwise set the destination route's argument and make it the active route on its depth
        route.Argument = argument;
        _activeRoutes[route.Depth] = route;

        // Raise the navigation completed event as navigation was successful
        NavigationCompleted?.Invoke(this, new RouterNavigationEventArgs(route));
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Validates the passed in routes and sets the active ones.
    /// </summary>
    /// <param name="routes">All of the provided application routes.</param>
    private void ValidateAndHandleRoutes(IReadOnlyCollection<Route> routes)
    {
        // If no routes provided or provided routes are not unique, fail
        if (routes.Count == 0)
        {
            throw new ArgumentException("At least one route must be provided.", nameof(routes));
        }

        if (routes.Distinct(new RouteEqualityComparer()).Count() != routes.Count)
        {
            throw new ArgumentException("Routes must be unique.", nameof(routes));
        }

        // Filter the routes by their router depths and go through each depth
        var routesByDepths = routes.GroupBy(r => r.Depth);
        foreach (var routesAtDepth in routesByDepths)
        {
            try
            {
                // Only one route must be initially active on its specific depth
                var initialRouteAtDepth = routesAtDepth.Single(r => r.IsInitial);

                // Add that route to the active routes for its depth
                _activeRoutes[initialRouteAtDepth.Depth] = initialRouteAtDepth;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("One and only one route must be set as initially active on its depth.", nameof(routes));
            }
        }
    }

    #endregion
}
