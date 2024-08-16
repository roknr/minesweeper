namespace Minesweeper.Core.Enums;

/// <summary>
/// The right click state of a tile.
/// </summary>
public enum TileMarkedState
{
    /// <summary>
    /// The tile has not been marked.
    /// </summary>
    Unmarked = 0,

    /// <summary>
    /// The tile has been marked as a flag.
    /// </summary>
    Flag = 1,

    /// <summary>
    /// The tile has been marked as a question mark.
    /// </summary>
    QuestionMark = 2
}
