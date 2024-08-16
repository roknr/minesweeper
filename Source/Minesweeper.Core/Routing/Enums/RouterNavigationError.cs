namespace Minesweeper.Core.Routing.Enums;

/// <summary>
/// The type of error that can occur during router navigation.
/// </summary>
public enum RouterNavigationError
{
    /// <summary>
    /// The destination route provided does not exist in the initially provided routes.
    /// </summary>
    NonExistingRoute = 0,

    /// <summary>
    /// The destination route provided is already the selected route on its depth.
    /// </summary>
    RouteAlreadyActive = 1
}
