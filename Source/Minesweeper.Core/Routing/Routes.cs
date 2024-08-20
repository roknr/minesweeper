using System.Collections;
using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Routing;

/// <summary>
/// The application <see cref="Route"/>s container that also provides access to specific routes
/// through static properties.
/// </summary>
public class Routes : IReadOnlyCollection<Route>
{
    #region Routes

    /// <summary>
    /// The start page route.
    /// </summary>
    public static Route StartPageRoute { get; } = new Route(ApplicationPage.Start, 0, isInitial: true);

    /// <summary>
    /// The game page route.
    /// </summary>
    public static Route GamePageRoute { get; } = new Route(ApplicationPage.Game, 0);

    /// <summary>
    /// Gets the number of routes in the application.
    /// </summary>
    public int Count => _routes.Count;

    #endregion

    #region Private members

    /// <summary>
    /// The application routes wrapped container.
    /// </summary>
    private readonly HashSet<Route> _routes =
    [
        StartPageRoute, GamePageRoute
    ];

    #endregion

    #region IEnumerable method implementations

    /// <summary>
    /// Returns an enumerator that iterates through the routes collection.
    /// </summary>
    public IEnumerator<Route> GetEnumerator() => _routes.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the routes collection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => _routes.GetEnumerator();

    #endregion
}
