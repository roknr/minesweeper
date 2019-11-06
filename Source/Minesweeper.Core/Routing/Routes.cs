using System.Collections;
using System.Collections.Generic;
using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Routing
{
    /// <summary>
    /// The application <see cref="Route"/>s container that also provides access to specific routes
    /// through static properties.
    /// </summary>
    public class Routes : IEnumerable<Route>
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

        #endregion

        #region Private members

        /// <summary>
        /// The application routes wrapped container.
        /// </summary>
        private readonly HashSet<Route> mRoutes = new HashSet<Route>
        {
            StartPageRoute, GamePageRoute
        };

        #endregion

        #region IEnumerable method implementations

        /// <summary>
        /// Returns an enumerator that iterates through the routes collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Route> GetEnumerator() => mRoutes.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the routes collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => mRoutes.GetEnumerator();

        #endregion
    }
}
