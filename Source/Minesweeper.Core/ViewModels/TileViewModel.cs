using Minesweeper.Core.Commands;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Events;

namespace Minesweeper.Core.ViewModels;

/// <summary>
/// The game tile view model.
/// </summary>
public class TileViewModel : ViewModelBase
{
    #region Events

    /// <summary>
    /// The event that is fired just before the tile is uncovered - just before the
    /// <see cref="IsUncovered"/> is updated.
    /// </summary>
    public event EventHandler? Uncovering;

    /// <summary>
    /// The event that is fired when the tile has been marked.
    /// </summary>
    public event EventHandler<TileMarkedEventArgs>? Marked;

    #endregion

    #region Public properties

    /// <summary>
    /// The row in which this tile is in.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// The column in which this tile is in.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// The number of adjacent bombs to this tile.
    /// </summary>
    public int AdjacentBombs { get; }

    /// <summary>
    /// The tile state.
    /// </summary>
    public TileState State { get; } = TileState.Empty;

    /// <summary>
    /// The marked state of the tile.
    /// </summary>
    public TileMarkedState MarkedState { get; private set; } = TileMarkedState.Unmarked;

    /// <summary>
    /// Value indicating whether this tile has already been uncovered.
    /// </summary>
    public bool IsUncovered { get; private set; } = false;

    /// <summary>
    /// Value indicating whether this tile can be uncovered or not. Can be uncovered if the
    /// tile is not marked and is not yet uncovered.
    /// </summary>
    public bool IsUncoverable => MarkedState == TileMarkedState.Unmarked && !IsUncovered;

    #region Commands

    /// <summary>
    /// The command that uncovers the tile.
    /// </summary>
    public IRelayCommand UncoverCommand { get; }

    /// <summary>
    /// The command that toggles the mark of the tile.
    /// </summary>
    public IRelayCommand ToggleMarkCommand { get; }

    #endregion

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TileViewModel"/> class
    /// with the specified row, column and state.
    /// </summary>
    /// <param name="row">The row in which this tile is in.</param>
    /// <param name="column">The column in which this tile is in.</param>
    /// <param name="adjacentBombs">The number of adjacent bombs to this tile.</param>
    /// <param name="state">The state of this tile.</param>
    public TileViewModel(int row, int column, int adjacentBombs, TileState state)
    {
        Row = row;
        Column = column;
        AdjacentBombs = adjacentBombs;
        State = state;

        UncoverCommand = new RelayCommand(_ => OnTileUncovering());
        ToggleMarkCommand = new RelayCommand(_ => OnTileToggledMark());
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Uncovers the tile in any situation without raising the <see cref="Uncovering"/> event
    /// (does not take into account the <see cref="IsUncoverable"/>).
    /// </summary>
    public void Uncover()
    {
        IsUncovered = true;
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Uncovers the tile and raises the <see cref="Uncovering"/> event, if the
    /// tile <see cref="IsUncoverable"/>.
    /// </summary>
    private void OnTileUncovering()
    {
        // Do nothing if the tile can't be uncovered
        if (!IsUncoverable)
        {
            return;
        }

        // Raise the uncovering event, just before updating the tile's uncovered value
        Uncovering?.Invoke(this, EventArgs.Empty);

        IsUncovered = true;
    }

    /// <summary>
    /// Toggles the <see cref="MarkedState"/> of this tile and raises the <see cref="Marked"/> event.
    /// </summary>
    private void OnTileToggledMark()
    {
        // Toggle the marked state based on the current marked state
        MarkedState = MarkedState switch
        {
            TileMarkedState.Unmarked => TileMarkedState.Flag,
            TileMarkedState.Flag => TileMarkedState.QuestionMark,
            _ => TileMarkedState.Unmarked
        };

        // And raise the event that the tile has been marked
        Marked?.Invoke(this, new TileMarkedEventArgs(MarkedState));
    }

    #endregion
}
