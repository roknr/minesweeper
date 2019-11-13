using System;

namespace Minesweeper.Core.Events
{
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
        /// Initializes a new instance of the <see cref="TileMarkedEventArgs"/> class
        /// with the specified flag indicating whether the player won or lost.
        /// </summary>
        /// <param name="playerWon">Flag, indicating whether the player won or lost the game.</param>
        public GameOverEventArgs(bool playerWon)
        {
            PlayerWon = playerWon;
        }
    }
}
