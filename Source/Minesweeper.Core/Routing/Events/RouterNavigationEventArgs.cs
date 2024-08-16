namespace Minesweeper.Core.Routing.Events;

/// <summary>
/// Represents the base router event arguments.
/// </summary>
public class RouterNavigationEventArgs : EventArgs
{
    /// <summary>
    /// The <see cref="Route"/> that is the destination of the navigation.
    /// </summary>
    public Route Destination { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouterNavigationEventArgs"/> class
    /// with the specified destination <see cref="Route"/>.
    /// </summary>
    /// <param name="destination">The navigation destination.</param>
    public RouterNavigationEventArgs(Route destination)
    {
        Destination = destination;
    }
}

/// <summary>
/// Represents the base router cancelable event arguments.
/// </summary>
public class RouterNavigationCancelEventArgs : RouterNavigationEventArgs
{
    /// <summary>
    /// Value indicating whether the event should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouterNavigationCancelEventArgs"/> class
    /// with the specified <see cref="Cancel"/> property or false by default, if not specified.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="cancel"></param>
    public RouterNavigationCancelEventArgs(Route destination, bool cancel = false) : base(destination)
    {
        Cancel = cancel;
    }
}
