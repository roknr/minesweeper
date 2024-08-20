using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Routing;

/// <summary>
/// Represents an application route.
/// </summary>
public class Route
{
    #region Public properties

    /// <summary>
    /// The type of page that this route displays.
    /// </summary>
    public ApplicationPage PageType { get; }

    /// <summary>
    /// The router depth at which this route is valid for.
    /// </summary>
    public uint Depth { get; }

    /// <summary>
    /// The value indicating whether this route should be initially selected on its router depth.
    /// </summary>
    public bool IsInitial { get; }

    /// <summary>
    /// The argument to use when navigating to this route.
    /// </summary>
    public object? Argument { get; set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Route"/> class.
    /// </summary>
    /// <param name="pageType">The type of page this route should display.</param>
    /// <param name="depth">The router depth at which this route is valid for.</param>
    /// <param name="isInitial">The value indicating whether this route should be
    /// initially selected on its router depth.</param>
    public Route(ApplicationPage pageType, uint depth, bool isInitial = false)
    {
        PageType = pageType;
        Depth = depth;
        IsInitial = isInitial;
    }

    #endregion
}
