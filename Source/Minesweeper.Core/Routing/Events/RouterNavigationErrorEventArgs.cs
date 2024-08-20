using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing.Enums;

namespace Minesweeper.Core.Routing.Events;

/// <summary>
/// Represents the event arguments for when the <see cref="IRouter"/> encountered an
/// error while navigating to a <see cref="Route"/>.
/// </summary>
public class RouterNavigationErrorEventArgs : RouterNavigationEventArgs
{
    /// <summary>
    /// The type of error that occurred during navigation.
    /// </summary>
    public RouterNavigationError Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouterNavigationErrorEventArgs"/> class.
    /// </summary>
    /// <param name="destination">The navigation destination.</param>
    /// <param name="error">The navigation error.</param>
    public RouterNavigationErrorEventArgs(Route destination, RouterNavigationError error) : base(destination)
    {
        Error = error;
    }
}
