using Minesweeper.Core.Enums;

namespace Minesweeper.Core.Events;

/// <summary>
/// Event arguments for the tile marked event.
/// </summary>
public class TileMarkedEventArgs : EventArgs
{
    /// <summary>
    /// The new marked state after the tile has been marked.
    /// </summary>
    public TileMarkedState NewMarkedState { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TileMarkedEventArgs"/> class
    /// with the specified new marked state.
    /// </summary>
    /// <param name="newMarkedState">The new tile marked state.</param>
    public TileMarkedEventArgs(TileMarkedState newMarkedState)
    {
        NewMarkedState = newMarkedState;
    }
}
