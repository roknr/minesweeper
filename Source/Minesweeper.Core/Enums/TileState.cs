namespace Minesweeper.Core.Enums
{
    /// <summary>
    /// The state of a game tile.
    /// </summary>
    public enum TileState
    {
        /// <summary>
        /// There is no bomb on the tile, neither are there any adjacent bombs to it.
        /// </summary>
        Empty,

        /// <summary>
        /// There is a bomb on the tile.
        /// </summary>
        Bomb,

        /// <summary>
        /// There are adjacent bombs to the tile.
        /// </summary>
        Number
    }
}
