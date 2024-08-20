using System.Collections.ObjectModel;
using System.Drawing;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Events;
using Minesweeper.Core.Extensions;
using Minesweeper.Core.ValueObjects;

namespace Minesweeper.Core.ViewModels;

/// <summary>
/// The game field view model.
/// </summary>
public class GameFieldViewModel : ViewModelBase
{
    #region Private members

    /// <summary>
    /// The game settings for this game.
    /// <para>Height == rows</para>
    /// <para>Width == columns</para>
    /// </summary>
    private readonly GameSettings _gameSettings;

    /// <summary>
    /// Flag indicating whether this is the first tile uncover click of the game. Initially it is.
    /// </summary>
    private bool _isFirstUncover = true;

    /// <summary>
    /// The list of bombs on the field.
    /// </summary>
    private readonly List<TileViewModel> _bombs;

    /// <summary>
    /// The list of tiles on the field, that have yet to be uncovered. When there are no more tiles
    /// in the list, the player wins the game.
    /// </summary>
    private readonly List<TileViewModel> _coveredTiles;

    /// <summary>
    /// The internal field reference of all tiles. Used for convenience and easier element access
    /// than <see cref="Field"/> which is a two dimensional <see cref="ObservableCollection{T}"/>.
    /// </summary>
    private readonly TileViewModel[,] _field;

    #endregion

    #region Events

    /// <summary>
    /// The event that is fired right after the game is started. This is after the user has uncovered
    /// the first tile.
    /// </summary>
    public event EventHandler? GameStarted;

    /// <summary>
    /// The event that gets fired after the game is over (either when the player won a.k.a.
    /// uncovered all the bombs, or the player lost a.k.a. clicked on a bomb).
    /// </summary>
    public event EventHandler<GameOverEventArgs>? GameOver;

    #endregion

    #region Public properties

    /// <summary>
    /// The number of bombs left to mark.
    /// </summary>
    public int BombsLeft { get; private set; }

    /// <summary>
    /// The tiles on the game field.
    /// </summary>
    public ObservableCollection<ObservableCollection<TileViewModel>> Field { get; private set; } = null!;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFieldViewModel"/> class
    /// with the specified game settings.
    /// </summary>
    /// <param name="gameSettings">The game settings for this game.</param>
    public GameFieldViewModel(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;

        _bombs = [];
        _coveredTiles = [];

        _field = new TileViewModel[_gameSettings.FieldHeight, _gameSettings.FieldWidth];

        // Initial bombs left are all of them
        BombsLeft = _gameSettings.NumberOfBombs;

        InitializeField();
    }

    #endregion

    #region Private helpers

    #region Field setup

    /// <summary>
    /// Initializes the game field, either with empty tiles (if matrices are not specified) or
    /// with data represented by the matrices.
    /// </summary>
    /// <param name="matrices">The field matrices that represent the state of the field - number of
    /// adjacent bombs and tile states.</param>
    private void InitializeField((int[,] AdjacencyMatrix, TileState[,] TileStates)? matrices = null)
    {
        // Empty the field if it was set before
        Field = [];

        for (var row = 0; row < _gameSettings.FieldHeight; row++)
        {
            Field.Add([]);

            for (var column = 0; column < _gameSettings.FieldWidth; column++)
            {
                TileViewModel tile;

                // If field matrices are not specified
                if (matrices == null)
                {
                    // Create empty tiles
                    tile = new TileViewModel(row, column, 0, TileState.Empty, canToggleMark: false);
                }
                else
                {
                    // Otherwise create the tiles from the specified matrices data
                    tile = new TileViewModel(
                        row,
                        column,
                        matrices.Value.AdjacencyMatrix[row, column],
                        matrices.Value.TileStates[row, column],
                        canToggleMark: true);

                    // If the tile has a bomb on it, add it to the list of bombs
                    if (tile.State == TileState.Bomb)
                    {
                        _bombs.Add(tile);
                    }
                    else
                    {
                        // Otherwise add it to the list of tiles that need to be uncovered
                        _coveredTiles.Add(tile);
                    }

                    // Only allow tile marking after first uncover, by listening for the tile's marked event
                    tile.Marked += (o, e) => OnTileMarked(e.NewMarkedState);
                }

                // Add the tile to the game field
                Field[row].Add(tile);
                _field[row, column] = tile;

                // Handle tile uncovering by listening for the tile's uncovering event
                tile.Uncovering += (o, e) =>
                {
                    var uncoveredTile = (TileViewModel)o!;
                    OnTileUncovering(uncoveredTile.Row, uncoveredTile.Column);
                };

                tile.AdjacentUncovering += (o, e) => OnTileAdjacentUncovering((TileViewModel)o!);
                tile.HighlightStarted += (o, e) => OnTileHighlight((TileViewModel)o!, highlight: true);
                tile.HighlightEnded += (o, e) => OnTileHighlight((TileViewModel)o!, highlight: false);
            }
        }
    }

    /// <summary>
    /// Sets up the game field by making sure the specified uncovered tile (identified by
    /// its row and column) is never a bomb.
    /// </summary>
    /// <param name="uncoveredTileRow">The row of the uncovered tile.</param>
    /// <param name="uncoveredTileColumn">The column of the uncovered tile.</param>
    private void SetupField(int uncoveredTileRow, int uncoveredTileColumn)
    {
        // Generate the places of bombs on the field, outside of the tile radius
        var bombField = GenerateBombs(uncoveredTileRow, uncoveredTileColumn);

        // Generate the tile state and adjacency matrices based on the generated bombs
        var matrices = GenerateTileStateAndAdjacencyMatrices(bombField);

        // Initialize the field once again, this time with actual data
        InitializeField(matrices);
    }

    /// <summary>
    /// Generates and returns a two dimensional array of the <see cref="_gameSettings"/> sizes that represents
    /// the game field, where a true represents a bomb and a false represents no bomb on the tile,
    /// represented with the corresponding coordinates. Also generates the bombs outside of the
    /// uncovered tile's adjacent radius.
    /// </summary>
    /// <param name="uncoveredTileRow">The row of the uncovered tile.</param>
    /// <param name="uncoveredTileColumn">The column of the uncovered tile.</param>
    /// <returns>A two dimensional array of <see cref="bool"/> where a <see cref="true"/> represents a bomb.</returns>
    private bool[,] GenerateBombs(int uncoveredTileRow, int uncoveredTileColumn)
    {
        // Represents the bombs on the field
        var bombs = new bool[_gameSettings.FieldHeight, _gameSettings.FieldWidth];
        var rand = new Random();

        var possibleCoordinatesCount = CalculatePossibleCoordinatesCount();
        var possibleCoordinates = new Point[possibleCoordinatesCount];
        var index = 0;

        // Add coordinates except for the uncovered tile and the ones adjacent to it
        for (var row = 0; row < _gameSettings.FieldHeight; row++)
        {
            for (var column = 0; column < _gameSettings.FieldWidth; column++)
            {
                if (Math.Abs(row - uncoveredTileRow) <= 1 && Math.Abs(column - uncoveredTileColumn) <= 1)
                {
                    continue;
                }

                possibleCoordinates[index++] = new Point(row, column);
            }
        }

        // Generate the specified number of bombs
        for (var i = 0; i < _gameSettings.NumberOfBombs; i++)
        {
            // Take a random coordinate by selecting a random index
            var randomIndex = rand.Next(0, possibleCoordinatesCount);
            var randomCoordinate = possibleCoordinates[randomIndex];

            // Place the bomb on the tile at the same coordinate
            bombs[randomCoordinate.X, randomCoordinate.Y] = true;

            // Swap the selected coordinate with the last available one
            possibleCoordinates[randomIndex] = possibleCoordinates[--possibleCoordinatesCount];
        }

        return bombs;

        int CalculatePossibleCoordinatesCount()
        {
            // The total number of tiles minus the ones excluded (uncovered and adjacent)
            var count = (_gameSettings.FieldHeight * _gameSettings.FieldWidth) - 9;

            if (IsTopOrBottomRow())
            {
                count += 3;
            }

            if (IsLeftMostOrRightMostColumn())
            {
                count += 3;
            }

            if (IsTopLeftCorner() || IsTopRightCorner() || IsBottomLeftCorner() || IsBottomRightCorner())
            {
                count--;
            }

            return count;

            bool IsTopOrBottomRow() => uncoveredTileRow is 0 || uncoveredTileRow == _gameSettings.FieldHeight - 1;
            bool IsLeftMostOrRightMostColumn() => uncoveredTileColumn is 0 || uncoveredTileColumn == _gameSettings.FieldWidth - 1;
            bool IsTopLeftCorner() => uncoveredTileRow is 0 && uncoveredTileColumn is 0;
            bool IsTopRightCorner() => uncoveredTileRow is 0 && uncoveredTileColumn == _gameSettings.FieldWidth - 1;
            bool IsBottomLeftCorner() => uncoveredTileRow == _gameSettings.FieldHeight - 1 && uncoveredTileColumn is 0;
            bool IsBottomRightCorner() => uncoveredTileRow == _gameSettings.FieldHeight - 1 && uncoveredTileColumn == _gameSettings.FieldWidth - 1;
        }
    }

    /// <summary>
    /// Generates and returns two matrices, based on the specified game field represented by bombs.
    /// <para>First matrix is a matrix of integer values that represent the number of adjacent to the tile
    /// at a corresponding coordinate.</para>
    /// <para>Second matrix is a matrix of tile states that represents the state of the tile at a
    /// corresponding coordinate.</para>
    /// </summary>
    /// <param name="bombs">The game field, representing bomb positions.</param>
    /// <returns></returns>
    private (int[,] AdjacencyMatrix, TileState[,] TileStates) GenerateTileStateAndAdjacencyMatrices(bool[,] bombs)
    {
        // Create the empty adjacency matrix and tile state matrix
        var adjacencyMatrix = new int[_gameSettings.FieldHeight, _gameSettings.FieldWidth];
        var tileStateMatrix = new TileState[_gameSettings.FieldHeight, _gameSettings.FieldWidth];

        // Go through the bomb field
        for (var row = 0; row < _gameSettings.FieldHeight; row++)
        {
            for (var column = 0; column < _gameSettings.FieldWidth; column++)
            {
                // Check each adjacent tile to the current tile
                foreach (var adjacentIsBomb in bombs.GetAdjacentElements(row, column))
                {
                    // And if its a bomb, update the current tile's adjacent bombs counter
                    if (adjacentIsBomb)
                    {
                        adjacencyMatrix[row, column]++;
                    }
                }

                // Set the appropriate tile state
                if (bombs[row, column])
                {
                    tileStateMatrix[row, column] = TileState.Bomb;
                }
                else if (adjacencyMatrix[row, column] > 0)
                {
                    tileStateMatrix[row, column] = TileState.Number;
                }
            }
        }

        return (adjacencyMatrix, tileStateMatrix);
    }

    #endregion

    #region Tile event handlers

    /// <summary>
    /// Handles the uncovering of a tile.
    /// </summary>
    /// <remarks>
    /// Use tile's row and column instead of the tile reference directly.
    /// See <seealso href="https://github.com/roknr/minesweeper/commit/ce80e698b337e5d916fe13e0d57ad3cf775b0d36"/>.
    /// </remarks>
    /// <param name="uncoveredTileRow">The row of the uncovered tile.</param>
    /// <param name="uncoveredTileColumn">The column of the uncovered tile.</param>
    private void OnTileUncovering(int uncoveredTileRow, int uncoveredTileColumn)
    {
        // If this is the first tile uncover
        if (_isFirstUncover)
        {
            // Set up the field, so that the first uncovered tile is never a bomb
            SetupField(uncoveredTileRow, uncoveredTileColumn);

            // It's not the first uncover anymore
            _isFirstUncover = false;

            // Raise the event that the game has started
            GameStarted?.Invoke(this, EventArgs.Empty);
        }

        // Get the tile that was uncovered from its coordinates
        var uncoveredTile = _field[uncoveredTileRow, uncoveredTileColumn];

        // It's not the first uncover, so check if the uncovered tile is a bomb
        if (uncoveredTile.State == TileState.Bomb)
        {
            // If so, the game is over so uncover all the bombs and raise the game over event
            UncoverBombs();
            GameOver?.Invoke(this, new GameOverEventArgs(playerWon: false, wasDirectBombClick: true));

            return;
        }

        // Otherwise, either an empty tile or a tile with a number was uncovered, so update it
        // and its adjacent tiles accordingly
        UncoverAdjacent(uncoveredTile);

        // Check if all tiles have been uncovered - if so, the player won
        if (_coveredTiles.Count != 0)
        {
            return;
        }

        GameOver?.Invoke(this, new GameOverEventArgs(playerWon: true, wasDirectBombClick: false));
    }

    /// <summary>
    /// Handles the marking of a tile.
    /// </summary>
    /// <param name="newMarkedState">The new marked tile state.</param>
    private void OnTileMarked(TileMarkedState newMarkedState)
    {
        switch (newMarkedState)
        {
            // If the tile was marked as a bomb, there is 1 bomb less to uncover
            case TileMarkedState.Flag:
            {
                BombsLeft--;
                break;
            }
            // If the tile was marked as a question mark, there is 1 possible bomb more
            case TileMarkedState.QuestionMark:
            {
                BombsLeft++;
                break;
            }
        }
    }

    /// <summary>
    /// Handles the recursive uncovering of a tile's adjacent tiles if the number of
    /// marked adjacent tiles is equal to the number of adjacent bombs.
    /// </summary>
    /// <param name="tile">The tile whose adjacent tiles to uncover.</param>
    private void OnTileAdjacentUncovering(TileViewModel tile)
    {
        if (_isFirstUncover || !tile.IsUncovered)
        {
            return;
        }

        var numberOfMarkedAdjacent = _field.GetAdjacentElements(tile.Row, tile.Column)
            .Count(x => x.MarkedState == TileMarkedState.Flag);

        if (tile.AdjacentBombs != numberOfMarkedAdjacent)
        {
            return;
        }

        var coveredUnmarkedAdjacent = _field.GetAdjacentElements(tile.Row, tile.Column)
            .Where(x => !x.IsUncovered && x.MarkedState is TileMarkedState.Unmarked);

        foreach (var adjacent in coveredUnmarkedAdjacent)
        {
            UncoverAdjacent(adjacent, uncoverBombs: true);
        }

        // Check if all tiles have been uncovered - if so, the player won
        if (_coveredTiles.Count != 0)
        {
            return;
        }

        GameOver?.Invoke(this, new GameOverEventArgs(playerWon: true, wasDirectBombClick: false));
    }

    /// <summary>
    /// Handles the highlighting of a tile's covered adjacent tiles.
    /// </summary>
    /// <param name="tile">The tile whose adjacent tiles to highlight.</param>
    /// <param name="highlight">Value to toggle the highlight.</param>
    private void OnTileHighlight(TileViewModel tile, bool highlight)
    {
        var coveredAdjacent = _field.GetAdjacentElements(tile.Row, tile.Column)
            .Where(x => !x.IsUncovered);

        foreach (var adjacent in coveredAdjacent)
        {
            adjacent.IsHighlighted = highlight;
        }
    }

    #endregion

    /// <summary>
    /// Uncovers the specified adjacent tile's neighbors recursively.
    /// </summary>
    /// <param name="adjacentTile">The adjacent tile to uncover.</param>
    /// <param name="uncoverBombs">Flag indicating whether to uncover bomb tiles.</param>
    private void UncoverAdjacent(TileViewModel adjacentTile, bool uncoverBombs = false)
    {
        // The tile can not be uncovered if it's marked, is a bomb or has been uncovered
        if (!adjacentTile.IsUncoverable || (!uncoverBombs && adjacentTile.State is TileState.Bomb))
        {
            return;
        }

        // Otherwise, the tile is either a number or empty, so uncover it and decrement the
        // number of covered tiles that must still be uncovered
        adjacentTile.Uncover();
        _coveredTiles.Remove(adjacentTile);

        // In case the tile is a number, do nothing more
        if (adjacentTile.State == TileState.Number)
        {
            return;
        }
        else if (uncoverBombs && adjacentTile.State is TileState.Bomb)
        {
            // In case a bomb has been uncovered the game is over so uncover all the bombs and raise the game over event
            UncoverBombs();
            GameOver?.Invoke(this, new GameOverEventArgs(playerWon: false, wasDirectBombClick: false));

            return;
        }

        // Otherwise update the tile's neighbors recursively
        foreach (var adjacent in _field.GetAdjacentElements(adjacentTile.Row, adjacentTile.Column))
        {
            UncoverAdjacent(adjacent);
        }
    }

    /// <summary>
    /// Uncovers all tiles that are bombs.
    /// </summary>
    private void UncoverBombs()
    {
        foreach (var bombTile in _bombs)
        {
            bombTile.Uncover();
        }
    }

    #endregion
}
