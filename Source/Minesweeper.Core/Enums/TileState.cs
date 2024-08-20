namespace Minesweeper.Core.Enums;

/// <summary>
/// The state of a game tile.
/// </summary>
public enum TileState
{
    /// <summary>
    /// There is no bomb on the tile, neither are there any adjacent bombs to it.
    /// </summary>
    Empty = 0,

    /// <summary>
    /// There is a bomb on the tile.
    /// </summary>
    Bomb = 1,

    /// <summary>
    /// There are adjacent bombs to the tile.
    /// </summary>
    Number = 2
}
