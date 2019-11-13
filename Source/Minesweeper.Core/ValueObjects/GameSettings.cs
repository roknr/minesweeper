namespace Minesweeper.Core.ValueObjects
{
    /// <summary>
    /// The game settings.
    /// </summary>
    public class GameSettings
    {
        #region Public properties

        /// <summary>
        /// The field width (columns).
        /// </summary>
        public int FieldWidth { get; }

        /// <summary>
        /// The field height (rows).
        /// </summary>
        public int FieldHeight { get; }

        /// <summary>
        /// The number of bombs on the field.
        /// </summary>
        public int NumberOfBombs { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettings"/> value object by providing the
        /// game field width, height and number of bombs on the field.
        /// </summary>
        /// <param name="fieldWidth">The field width.</param>
        /// <param name="fieldHeight">The field height.</param>
        /// <param name="numberOfBombs">The number of bombs on the field.</param>
        public GameSettings(int fieldWidth, int fieldHeight, int numberOfBombs)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            NumberOfBombs = numberOfBombs;
        }

        #endregion
    }
}
