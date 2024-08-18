namespace Minesweeper.Core.Events;

/// <summary>
/// Event arguments for the game over event.
/// </summary>
public class GameOverEventArgs : EventArgs
{
    /// <summary>
    /// Value indicating whether the player won or lost the game.
    /// </summary>
    public bool PlayerWon { get; }

    /// <summary>
    /// Value indicating whether the cause of the game over was a direct click on a bomb.
    /// </summary>
    public bool WasDirectBombClick { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TileMarkedEventArgs"/> class
    /// with the specified flag indicating whether the player won or lost.
    /// </summary>
    /// <param name="playerWon">Flag, indicating whether the player won or lost the game.</param>
    /// <param name="wasDirectBombClick">Flag indicating whether the cause of the game over was a direct click on a bomb.</param>
    public GameOverEventArgs(bool playerWon, bool wasDirectBombClick)
    {
        PlayerWon = playerWon;
        WasDirectBombClick = wasDirectBombClick;
    }
}
