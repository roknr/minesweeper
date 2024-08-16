using System.Collections.ObjectModel;
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
                    tile = new TileViewModel(row, column, 0, TileState.Empty);
                    Field[row].Add(tile);
                }
                else
                {
                    // Otherwise create the tiles from the specified matrices data
                    tile = new TileViewModel(
                        row,
                        column,
                        matrices.Value.AdjacencyMatrix[row, column],
                        matrices.Value.TileStates[row, column]);

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

                    // Add the tile to the game field
                    Field[row].Add(tile);
                    _field[row, column] = tile;
                }

                // Handle tile uncovering by listening for the tile's uncovering event
                tile.Uncovering += (o, e) =>
                {
                    var uncoveredTile = (TileViewModel)o!;
                    OnTileUncovering(uncoveredTile.Row, uncoveredTile.Column);
                };
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
    /// <returns></returns>
    private bool[,] GenerateBombs(int uncoveredTileRow, int uncoveredTileColumn)
    {
        // Represents the bombs on the field
        var bombs = new bool[_gameSettings.FieldHeight, _gameSettings.FieldWidth];
        var rand = new Random();

        // Represents the possible coordinates where bombs can be placed
        var possibleCoordinates = new List<Tuple<int, int>>();

        // Create all coordinates
        for (var row = 0; row < _gameSettings.FieldHeight; row++)
        {
            for (var column = 0; column < _gameSettings.FieldWidth; column++)
            {
                possibleCoordinates.Add(new Tuple<int, int>(row, column));
            }
        }

        // From all coordinates, remove the uncovered tile and the ones adjacent to it
        possibleCoordinates.Remove(new Tuple<int, int>(uncoveredTileRow, uncoveredTileColumn));
        bombs.ForEachAdjacent(uncoveredTileRow, uncoveredTileColumn, (_, row, column) =>
        {
            // Use "new" because Tuple comparison works by comparing component values instead of object references
            possibleCoordinates.Remove(new Tuple<int, int>(row, column));
        });

        // Generate the specified number of bombs
        for (var i = 0; i < _gameSettings.NumberOfBombs; i++)
        {
            // Take a random coordinate
            var randomCoordinate = possibleCoordinates[rand.Next(0, possibleCoordinates.Count)];

            // Place the bomb on the tile at the same coordinate
            bombs[randomCoordinate.Item1, randomCoordinate.Item2] = true;

            // Remove the coordinate from the possible ones since there's now a bomb there
            possibleCoordinates.Remove(randomCoordinate);
        }

        return bombs;
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
                bombs.ForEachAdjacent(row, column, (adjacentIsBomb, _, __) =>
                {
                    // And if its a bomb, update the current tile's adjacent bombs counter
                    if (!adjacentIsBomb)
                    {
                        return;
                    }

                    adjacencyMatrix[row, column]++;
                });

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
            GameOver?.Invoke(this, new GameOverEventArgs(playerWon: false));

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

        GameOver?.Invoke(this, new GameOverEventArgs(playerWon: true));
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

    #endregion

    /// <summary>
    /// Uncovers the specified adjacent tile's neighbors recursively.
    /// </summary>
    /// <param name="adjacentTile">The adjacent tile to uncover.</param>
    private void UncoverAdjacent(TileViewModel adjacentTile)
    {
        // The tile can not be uncovered if it's marked, is a bomb or has been uncovered
        if (!adjacentTile.IsUncoverable || adjacentTile.State == TileState.Bomb)
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

        // Otherwise update the tile's neighbors recursively
        _field.ForEachAdjacent(adjacentTile.Row, adjacentTile.Column, (newAdjacentTile, _, __) => UncoverAdjacent(newAdjacentTile));
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
