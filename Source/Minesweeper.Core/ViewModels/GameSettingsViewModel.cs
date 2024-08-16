namespace Minesweeper.Core.ViewModels;

/// <summary>
/// The game settings view model.
/// </summary>
public class GameSettingsViewModel : ViewModelBase
{
    #region Private members

    /// <summary>
    /// The width of the game field.
    /// </summary>
    private int _fieldWidth;

    /// <summary>
    /// The height of the game field.
    /// </summary>
    private int _fieldHeight;

    /// <summary>
    /// The number of bombs on the field.
    /// </summary>
    private int _numberOfBombs;

    #endregion

    #region Public properties

    #region Static constants

    /// <summary>
    /// The minimum width of the game field.
    /// </summary>
    public const int MinimumWidth = 9;

    /// <summary>
    /// The maximum width of the game field.
    /// </summary>
    public const int MaximumWidth = 32;

    /// <summary>
    /// The minimum height of the game field.
    /// </summary>
    public const int MinimumHeight = 9;

    /// <summary>
    /// The maximum height of the game field.
    /// </summary>
    public const int MaximumHeight = 20;

    /// <summary>
    /// The minimum number of bombs on the field.
    /// </summary>
    public const int MinimumBombs = 10;

    /// <summary>
    /// Returns a new instance of the <see cref="GameSettingsViewModel"/> with the beginner difficulty settings.
    /// </summary>
    public static GameSettingsViewModel BeginnerDifficulty => new(9, 9, 10);

    /// <summary>
    /// Returns a new instance of the <see cref="GameSettingsViewModel"/> with the intermediate difficulty settings.
    /// </summary>
    public static GameSettingsViewModel IntermediateDifficulty => new(16, 16, 40);

    /// <summary>
    /// Returns a new instance of the <see cref="GameSettingsViewModel"/> with the expert difficulty settings.
    /// </summary>
    public static GameSettingsViewModel ExpertDifficulty => new(30, 16, 99);

    #endregion

    /// <summary>
    /// The maximum number of bombs that can be on the field.
    /// Changes based on the values of <see cref="FieldWidth"/> and <see cref="FieldHeight"/>.
    /// </summary>
    public int MaximumBombs => (FieldWidth - 1) * (FieldHeight - 1);

    /// <summary>
    /// The game field width.
    /// </summary>
    public int FieldWidth
    {
        get => _fieldWidth;
        set
        {
            // Handle field width to be within allowed range
            _fieldWidth = value switch
            {
                < MinimumWidth => MinimumWidth,
                > MaximumWidth => MaximumWidth,
                _ => value
            };

            // Handle a possible bomb overflow, since the field dimensions have changed
            CheckAndHandleBombOverflow();
        }
    }

    /// <summary>
    /// The game field height.
    /// </summary>
    public int FieldHeight
    {
        get => _fieldHeight;
        set
        {
            // Handle field height to be within allowed range
            _fieldHeight = value switch
            {
                < MinimumHeight => MinimumHeight,
                > MaximumHeight => MaximumHeight,
                _ => value
            };

            // Handle a possible bomb overflow, since the field dimensions have changed
            CheckAndHandleBombOverflow();
        }
    }

    public int NumberOfBombs
    {
        get => _numberOfBombs;
        set
        {
            // Handle number of bombs to be within allowed range
            if (value < MinimumBombs)
            {
                _numberOfBombs = MinimumBombs;
            }
            else if (value > MaximumBombs)
            {
                _numberOfBombs = MaximumBombs;
            }
            else
            {
                _numberOfBombs = value;
            }
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GameSettingsViewModel"/> class
    /// with the specified settings.
    /// </summary>
    /// <param name="fieldWidth">The initial game field width.</param>
    /// <param name="fieldHeight">The initial game field height.</param>
    /// <param name="numberOfBombs">The initial number of bombs on the field.</param>
    public GameSettingsViewModel(int fieldWidth, int fieldHeight, int numberOfBombs)
    {
        FieldWidth = fieldWidth;
        FieldHeight = fieldHeight;
        NumberOfBombs = numberOfBombs;
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Checks if <see cref="NumberOfBombs"/> is outside the possible bomb range and, if so,
    /// sets it to the <see cref="MinimumBombs"/> or <see cref="MaximumBombs"/>.
    /// </summary>
    private void CheckAndHandleBombOverflow()
    {
        if (NumberOfBombs > MaximumBombs)
        {
            NumberOfBombs = MaximumBombs;
        }
        else if (NumberOfBombs < MinimumBombs)
        {
            NumberOfBombs = MinimumBombs;
        }
    }

    #endregion
}
